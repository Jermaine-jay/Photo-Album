using AutoMapper;
using ImageApp.BLL.Extensions;
using ImageApp.BLL.Interface;
using ImageApp.BLL.Models;
using ImageApp.DAL.Enums;
using ImageApp.DAL.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using User = ImageApp.DAL.Entities.User;

namespace ImageApp.BLL.Implementation
{
	public class UserServices : IUserServices
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly IMapper _mapper;
		private readonly IRepository<User> _userRepo;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IAuthenticationService _authenticationService;

		public UserServices(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, IUnitOfWork unitOfWork, IAuthenticationService authenticationService)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_unitOfWork = unitOfWork;
			_userRepo = _unitOfWork?.GetRepository<User>();
			_mapper = mapper;
			_authenticationService = authenticationService;
		}

		public async Task<(bool successful, string msg)> RegisterAdmin(IUrlHelper urlHelper, RegisterVM register)
		{
			var (newUser, msg) = await CreateAUser(register);
			if (newUser == null)
			{
				return (false, msg);
			}
			IdentityResult result = await _userManager.CreateAsync(newUser, register.Password);

			if (result.Succeeded)
			{
				_ = _authenticationService.RegistrationMail(urlHelper, newUser);

				await _userManager.AddToRoleAsync(newUser, "Admin");
				await _signInManager.SignInAsync(newUser, isPersistent: false);
				return result.Succeeded ? (true, "User created successfully!, Verification Mail Sent") : (false, "Failed to create User, Couldn't Send Mail");
			}
			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{
					return (false, $"Failed to create User.{error.Description}");
				}
			}
			return (false, $"Failed to create User");
		}

		public async Task<(bool successful, string msg)> RegisterUser(IUrlHelper urlHelper, RegisterVM register)
		{
			var (newUser, msg )= await CreateAUser(register);
			if (newUser == null)
			{
				return (false, msg);
			}
			IdentityResult result = await _userManager.CreateAsync(newUser, register.Password);

			if (result.Succeeded)
			{
				_ = _authenticationService.RegistrationMail(urlHelper, newUser);

				await _userManager.AddToRoleAsync(newUser, "User");
				await _signInManager.SignInAsync(newUser, isPersistent: false);
				return result.Succeeded ? (true, "User created successfully!, Verification Mail Sent") : (false, "Failed to create User, Couldn't Send Mail");
			}
			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{
					return (false, $"Failed to create User.{error.Description}");
				} 
			}         
            return (false, $"Failed to create User");
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

		public async Task<(bool successful, string msg)> Update(UserVM model)
		{

			var user = await _userRepo.GetSingleByAsync(u => u.Id == model.Id);
			var verify = await _authenticationService.VerifyEmail(model.Email);
			if (verify == false)
			{
				return (false, $"Invalid Email Address");
			}

			var userupdate = _mapper.Map(model, user);
			var rowChanges = await _userRepo.UpdateAsync(userupdate);

			return rowChanges != null ? (true, $"User detail update was successful!") : (false, "Failed To save changes!");
		}

		public async Task<(bool successful, string msg)> DeleteAsync(string userId)
		{
			var user = await _userRepo.GetSingleByAsync(u => u.Id == userId);
			if (user == null)
			{
				return (false, $"User with user:{user.UserName} wasn't found");
			}

			await _userRepo.DeleteAsync(user);
			return await _unitOfWork.SaveChangesAsync() >= 0 ? (true, $"{user.UserName} deleted") : (false, $"Delete Failed");
		}

		public async Task<(User, string msg)> CreateAUser(RegisterVM register)
		{
			string[] dateComponents = register.DateOfBirth.Split('-');
			int year = int.Parse(dateComponents[0]);
			int age = DateTime.Now.Year - year;

			var verify = await _authenticationService.VerifyEmail(register.Email);
			if (verify == false)
			{
				return (null, "Invalid Email Address");
			}

			User newUser = new User
			{
				UserName = register.Username,
				Email = register.Email,
				PhoneNumber = register.PhoneNumber,
				Address = register.Address,
				Gender = Enum.Parse<Gender>(register.Gender),
				DateOfBirth = DateTime.Parse(register.DateOfBirth),
				Age = age.ToString()
			};
			return (newUser, "Valid Email"); ;
		}

		public async Task<ProfileVM> UserProfileAsync(string userId)
		{
			var u = await _userRepo.GetSingleByAsync(u => u.Id == userId);
			var useres = new ProfileVM()
			{
				Id = u.Id,
				UserName = u.UserName,
				Email = u.Email,
				PhoneNumber = u.PhoneNumber,
				Address = u.Address,
				Age = u.Age,
				Gender = u.Gender.ToString(),
				DateOfBirth = u.DateOfBirth.ToString("dd MMMM yyyy"),
				ProfileImagePath = u.ProfileImagePath

			};
			return useres;
		}

		public async Task<ProfileVM> GetUser(string Id)
		{
			var user = await _userRepo.GetSingleByAsync(u => u.Id == Id);
			var Auser = _mapper.Map<ProfileVM>(user);
			return Auser;
		}

		public async Task<IEnumerable<ProfileVM>> GetUsers()
		{
			var users = await _userRepo.GetAllAsync();
			var userViewModels = users.Select(u => new ProfileVM
			{
				Id = u.Id,
				UserName = u.UserName,
				Email = u.Email,
				DateOfBirth = u.DateOfBirth.ToString("d"),
				Gender = u.Gender.ToString(),
				Age = u.Age,
				PhoneNumber = u.PhoneNumber,
				Address= u.Address,
			});
			return userViewModels;
		}
	}
}
