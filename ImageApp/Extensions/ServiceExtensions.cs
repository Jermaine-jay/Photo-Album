using ImageApp.BLL.Implementation;
using ImageApp.BLL.Interface;
using ImageApp.DAL.DataBase;
using ImageApp.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace ImageApp.Extensions
{
    public static class ServiceExtensions
    {
        private static readonly IConfiguration configuration;
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IUploadImageService, UploadImageService>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IPropertyService, PropertyService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IRecoveryService, RecoveryService>();
            services.AddScoped<IGenerateEmailVerificationPage, GenerateEmailVerificationPage>();
            services.AddHttpContextAccessor();

            //services.Configure<EmailSenderOptions>(configuration.GetSection("EmailSenderOptions"));
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ImageAppDbContext>()
                .AddDefaultTokenProviders();

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
    }
}
