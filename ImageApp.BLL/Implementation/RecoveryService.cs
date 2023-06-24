using AutoMapper;
using ImageApp.BLL.Extensions;
using ImageApp.BLL.Interface;
using ImageApp.BLL.Models;
using ImageApp.DAL.Entities;
using ImageApp.DAL.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ImageApp.BLL.Implementation
{
	public class RecoveryService : IRecoveryService
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly IMapper _mapper;
		private readonly IRepository<User> _userRepo;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IAuthenticationService _authenticationService;
		private readonly HttpContext _httpContext;
		private RoleManager<IdentityRole> _roleManager { get; }

		public RecoveryService(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IUnitOfWork unitOfWork, IAuthenticationService authenticationService)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
			_unitOfWork = unitOfWork;
			_userRepo = _unitOfWork?.GetRepository<User>();
			_mapper = mapper;
			_authenticationService = authenticationService;
		}
		public Task<(bool successful, string msg)> ChangeEmail(string userId, string code)
		{
			throw new NotImplementedException();
		}

		public async Task<(bool successful, string msg)> ForgotPassword(IUrlHelper urlHelper, string Protocol, string Email)
		{
			var user = await _userManager.FindByEmailAsync(Email);
			if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
			{
				return (false, "User doesn't exist");
			}

			var code = await _userManager.GeneratePasswordResetTokenAsync(user);
			var callbackUrl = urlHelper.Action("ResetPassword", "User", new { userId = user.Id, code }, protocol: Protocol);

			await _authenticationService.SendEmailAsync(Email, "Reset Password", GenerateEmailVerificationPage.PasswordResetPage(callbackUrl));
			return (true, "Reset Password Email Sent");
		}

		public Task<(bool successful, string msg)> ResetEmail(string userId, string code)
		{
			throw new NotImplementedException();
		}

		public async Task<(bool successful, string msg)> ResetPassword(ResetPasswordVM model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user == null)
			{
				return (false, "User does not exist!");
			}

			var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
			if (result.Succeeded)
			{
				return (true, "Password Reset Complete");
			}
			else
			{
				foreach (var error in result.Errors)
				{
					return (false, $"Couldn't reset password, {error.Description}");
				}
			}
			throw new NotImplementedException();
		}
	}
}
