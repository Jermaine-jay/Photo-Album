using ImageApp.BLL.Interface;
using ImageApp.BLL.Models;
using ImageApp.DAL.Entities;
using ImageApp.DAL.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace ImageApp.BLL.Implementation
{
	public class RecoveryService : IRecoveryService
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly IRepository<User> _userRepo;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IServiceFactory _serviceFactory;

		private readonly LinkGenerator _linkGenerator;
		private readonly IHttpContextAccessor _httpContextAccessor;


		public RecoveryService(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor, IServiceFactory serviceFactory, UserManager<User> userManager, SignInManager<User> SignInManager, IUnitOfWork unitOfWork)
		{
			_linkGenerator = linkGenerator;
			_serviceFactory = serviceFactory;
			_httpContextAccessor = httpContextAccessor;
			_userManager = userManager;
			_signInManager = SignInManager;
			_unitOfWork = unitOfWork;
			_userRepo = _unitOfWork?.GetRepository<User>();
		}


		public async Task<(bool successful, string msg)> ChangeEmail(string userId, string code)
		{
			var user = await _userManager.FindByIdAsync(userId);
			var result = await _userManager.ChangeEmailAsync(user, user.Email, code);

			if (result.Succeeded)
			{
				return (true, "Email Changed Successfully");
			}
			return (true, "Failed to change Email");
		}



		public async Task<string> ResetEmailToken(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user != null)
			{
				var code = await _userManager.GenerateChangeEmailTokenAsync(user, user.Email);
				return code;
			}
			return null;
		}



		public async Task<(bool successful, string msg)> ForgotPassword(ForgotPasswordVM model)
		{

			var verify = await _serviceFactory.GetService<IAuthenticationService>().VerifyEmail(model.Email);
			if (verify == false)
			{
				return (false, "Invalid Email Address");
			}

			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
			{
				return (false, "User doesn't exist");
			}

			var code = await _userManager.GeneratePasswordResetTokenAsync(user);
			var callbackUrl = _linkGenerator.GetUriByAction(_httpContextAccessor.HttpContext, action:"ResetPassword", controller:"User", values:new { UserId = user.Id, code });
			var page = _serviceFactory.GetService<IGenerateEmailVerificationPage>().PasswordResetPage(callbackUrl);

			await _serviceFactory.GetService<IAuthenticationService>().SendEmailAsync(model.Email, "Reset Password", page);
			return (true, "Reset Password Email Sent");
		}



		public async Task<(bool successful, string msg)> ResetPassword(ResetPasswordVM model)
		{
			var user = await _userManager.FindByIdAsync(model.UserId);
			if (user != null)
			{
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
			}
			return (false, "User does not exist!");
		}


		public async Task<(bool successful, string msg)> ChangeDetailToken(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
			{
				return (false, "User doesn't exist");
			}

			var token = await _userManager.GenerateUserTokenAsync(user, "PasswordlessLoginTotpProvider", "passwordless-auth");
			var page = _serviceFactory.GetService<IGenerateEmailVerificationPage>().ChangePasswordPage(token);

			await _serviceFactory.GetService<IAuthenticationService>().SendEmailAsync(user.Email, "Change Details", page);
			return (true, "Change Detail Email Sent");
		}



		public async Task<(bool successful, string msg)> VerifyChangeDetailToken(ConfirmTokenVM model)
		{
			var user = await _userManager.FindByIdAsync(model.UserId);
			if (user == null)
			{
				return (false, "User not forund.");
			}

			var isValid = await _userManager.VerifyUserTokenAsync(user, "PasswordlessLoginTotpProvider", "passwordless-auth", model.Token);
			if (isValid)
			{
				return (true, "Token verified");
			}
			return (false, "Verification failed");

		}



		public async Task<(bool successful, string msg)> ChangePassword(ChangePasswordVM model)
		{
			var user = await _userManager.FindByIdAsync(model.UserId);
			if (user != null)
			{
				var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
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
			}
			return (false, "User does not exist!");
		}
	}
}
