using ImageApp.BLL.Implementation;
using ImageApp.BLL.Interface;
using ImageApp.DAL.DataBase;
using ImageApp.DAL.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ImageApp.Extensions
{
	public static class ServiceExtensions
	{
		public static void RegisterServices(this IServiceCollection services)
		{
			services.AddScoped<IUploadImageService, UploadImageService>();
			services.AddScoped<IUserServices, UserServices>();
			services.AddScoped<IPropertyService, PropertyService>();
			services.AddScoped<IAuthenticationService, AuthenticationService>();
			services.AddScoped<IRecoveryService, RecoveryService>();
			services.AddScoped<IGenerateEmailVerificationPage, GenerateEmailVerificationPage>();
			services.AddScoped<IServiceFactory, ServiceFactory>();
			services.AddHttpContextAccessor();
			services.Configure<DataProtectionTokenProviderOptions>(x => x.TokenLifespan = TimeSpan.FromMinutes(10));
		}



		public static void ConfigureIdentity(this IServiceCollection services)
		{
			services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
				.AddEntityFrameworkStores<ImageAppDbContext>()
				.AddDefaultTokenProviders()
				.AddPasswordlessLoginTotpTokenProvider();


			services.Configure<IdentityOptions>(opt =>
			{
				opt.Password.RequiredLength = 6;
				opt.Password.RequireNonAlphanumeric = false;
				opt.Password.RequireDigit = true;
				opt.Password.RequireLowercase = true;
				opt.Password.RequireUppercase = false;
				opt.User.RequireUniqueEmail = true;
				opt.Lockout.MaxFailedAccessAttempts = 3;
				opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);

			});
			services.AddHttpContextAccessor();
			services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = "/User/SignIn";
			});
		}



		public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = configuration["Jwt:Issuer"],
					ValidAudience = configuration["Jwt:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
				};
			});
		}

		public static void Configure(IServiceProvider serviceProvider)
		{
			CreateRoles(serviceProvider).Wait();
		}

		private static async Task CreateRoles(IServiceProvider serviceProvider)
		{
			var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();


			if (!await roleManager.RoleExistsAsync("Admin"))
			{

				var role = new IdentityRole("Admin");
				await roleManager.CreateAsync(role);
			}

			if (!await roleManager.RoleExistsAsync("User"))
			{

				var role = new IdentityRole("User");
				await roleManager.CreateAsync(role);
			}
		}


		public class PasswordlessLoginTotpTokenProvider<TUser> : TotpSecurityStampBasedTokenProvider<TUser> where TUser : class
		{
			public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
			{
				return Task.FromResult(false);
			}

			public override async Task<string> GetUserModifierAsync(string purpose, UserManager<TUser> manager, TUser user)
			{
				var email = await manager.GetEmailAsync(user);
				return "PasswordlessLogin:" + purpose + ":" + email;
			}
		}
	}
}
