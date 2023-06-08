using AutoMapper;
using ImageApp.BLL.Interface;
using ImageApp.BLL.Models;
using ImageApp.DAL.Entities;
using ImageApp.DAL.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ImageApp.BLL.Implementation
{
	public class UserServices : IUserServices
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly IMapper _mapper;
		private readonly IRepository<User> _userRepo;

		private RoleManager<IdentityRole> _roleManager { get; }

		public UserServices(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
			_mapper = mapper;
		}

		public async Task<(bool successful, string msg)> RegisterAdmin(RegisterVM register)
		{
			string[] dateComponents = register.DateOfBirth.Split('/');

			int year = int.Parse(dateComponents[2]);
			int age = DateTime.Now.Year - year;
			register.Age = age.ToString();

			var newUser = _mapper.Map<User>(register);
			IdentityResult result = await _userManager.CreateAsync(newUser, register.Password);

			await _userManager.AddToRoleAsync(newUser, "Admin");
			await _signInManager.SignInAsync(newUser, isPersistent: false);

			return result.Succeeded ? (true, "Admin created successfully!") : (false, "Failed to create Admin");
		}

		public async Task<(bool successful, string msg)> RegisterUser(RegisterVM register)
		{
			string[] dateComponents = register.DateOfBirth.Split('/');

			int year = int.Parse(dateComponents[2]);
			int age = DateTime.Now.Year - year;
			register.Age = age.ToString();

			var newUser = _mapper.Map<User>(register);
			IdentityResult result = await _userManager.CreateAsync(newUser, register.Password);

			await _userManager.AddToRoleAsync(newUser, "User");

			await _signInManager.SignInAsync(newUser, isPersistent: false);

			return result.Succeeded ? (true, "User created successfully!") : (false, "Failed to create User");
		}

		public async Task<(bool successful, string msg)> SignIn(SignInVM signIn)
		{
			User user;
			if (signIn.UsernameOrEmail.Contains("@"))
			{
				user = await _userManager.FindByEmailAsync(signIn.UsernameOrEmail);
			}
			else
			{
				user = await _userManager.FindByNameAsync(signIn.UsernameOrEmail);
			}

			if (user != null)
			{
				var result = await _signInManager.PasswordSignInAsync(user, signIn.Password, signIn.RememberMe, true);

				return result.Succeeded ? (true, $"{user.UserName} logged in successfully!") : (false, "Failed to login");
			}
			return (false, "User does not exist");
		}

		public async Task<(bool successful, string msg)> SignOut()
		{
			await _signInManager.SignOutAsync();
			return (true, $"logged out successfully!");
		}
		public async Task<(bool successful, string msg)> Update(RegisterVM model)
		{

			var user = await _userRepo.GetSingleByAsync(u => u.Id == model.Id);
			if (user == null)
			{
				return (false, $"User with ID:{model.Id} wasn't found");
			}

			var userupdate = _mapper.Map(model, user);
			var rowChanges = await _userRepo.UpdateAsync(userupdate);

			return rowChanges != null ? (true, $"User detail update was successful!") : (false, "Failed To save changes!");

		}

		public async Task<UserVM> GetUser(string? userId)
		{
			var user = await _userRepo.GetSingleByAsync(u => u.Id == userId);
			var Auser = _mapper.Map<UserVM>(user);
			return Auser;
		}

		public async Task<IEnumerable<PictureVM>> GetUsersWithTasksAsync(string userId)
		{
			User? user = await _userRepo.GetSingleByAsync(u => u.Id == userId, include: u => u.Include(x => x.Pictures), tracking: true);
			IEnumerable<PictureVM>? UwT = user?.Pictures?.Select(t => new PictureVM
			{

				Id = t.Id,
				Name = t.Name,
				Description = t.Description,
				ImageFile = t.ImageFile,
			});
			return UwT;
		}
	}
}
