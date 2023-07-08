using ImageApp.BLL.Interface;
using ImageApp.BLL.Models;
using ImageApp.DAL.Entities;
using ImageApp.DAL.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace ImageApp.BLL.Implementation
{
    public class RecoveryService : IRecoveryService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IRepository<User> _userRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticationService _authenticationService;
        private readonly IGenerateEmailVerificationPage _generateEmailVerificationPage;

        public RecoveryService(UserManager<User> userManager, SignInManager<User> SignInManager, IUnitOfWork unitOfWork, IGenerateEmailVerificationPage Page, IAuthenticationService Service)
        {
            _userManager = userManager;
            _signInManager = SignInManager;
            _unitOfWork = unitOfWork;
            _userRepo = _unitOfWork?.GetRepository<User>();
            _authenticationService = Service;
            _generateEmailVerificationPage = Page;
        }

        public Task<(bool successful, string msg)> ChangeEmail(string userId, string code)
        {
            throw new NotImplementedException();
        }


        public async Task<(bool successful, string msg)> ForgotPassword(IUrlHelper urlHelper, ForgotPasswordVM model)
        {
            var verify = await _authenticationService.VerifyEmail(model.Email);
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
            var callbackUrl = urlHelper.Action("ResetPassword", "User", new { UserId = user.Id, code }, protocol: "https");

            await _authenticationService.SendEmailAsync(model.Email, "Reset Password", _generateEmailVerificationPage.PasswordResetPage(callbackUrl));
            return (true, "Reset Password Email Sent");
        }



        public Task<(bool successful, string msg)> ResetEmail(string userId, string code)
        {
            throw new NotImplementedException();
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


        public async Task<(bool successful, string msg)> VerifyChangePasswordToken(ConfirmTokenVM model)
        {
            //var user = _signInManager.GetTwoFactorAuthenticationUserAsync();
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return (false, "Unable to load two-factor authentication user.");
            }

			//var code1 = model.Token.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);
			//var isTokenValid = await _userManager.VerifyUserTokenAsync(user, "Email", "passwordless-auth\"", model.Token);
			//var result = await _userManager.VerifyUserTokenAsync(user,"Email", "Change Password",model.Token);
			//var result = await _signInManager.("Email", model.Token, isPersistent: false, rememberClient: false);

			var isValid = await _userManager.VerifyUserTokenAsync(user, "PasswordlessLoginTotpProvider", "passwordless-auth", model.Token);
			if (isValid)
            {
                return (true, "Token verified");
            }
            return (false, "Verification failed");

			/*var result = await _signInManager.TwoFactorSignInAsync("Email", twoStepModel.TwoFactorCode, twoStepModel.RememberMe, rememberClient: false);
			if (result.Succeeded)
			{
                return null;
			}*/
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



        public async Task<(bool successful, string msg)> ChangePasswordToken(IUrlHelper urlHelper, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return (false, "User doesn't exist");
            }

			//var code = await _userManager.GenerateUserTokenAsync(user, "Email", "passwordless-auth");

			var token = await _userManager.GenerateUserTokenAsync(user, "PasswordlessLoginTotpProvider", "passwordless-auth");

			//var code = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
			//var code = await _userManager.ResetAuthenticatorKeyAsync(user);
			//var callbackUrl = urlHelper.Action("ChangePassword", "User", new { UserId = user.Id, code }, protocol: "https");

			await _authenticationService.SendEmailAsync(user.Email, "Change Password", _generateEmailVerificationPage.ChangePasswordPage(token));
            return (true, "Change Password Email Sent");
        }
    }
}
