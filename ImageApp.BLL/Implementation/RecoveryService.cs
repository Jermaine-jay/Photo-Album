using ImageApp.BLL.Interface;
using ImageApp.BLL.Models;
using ImageApp.DAL.Entities;
using ImageApp.DAL.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ImageApp.BLL.Implementation
{
    public class RecoveryService : IRecoveryService
    {
        private readonly UserManager<User> _userManager;
        private readonly IRepository<User> _userRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticationService _authenticationService;
        private readonly IGenerateEmailVerificationPage _generateEmailVerificationPage;

        public RecoveryService(UserManager<User> userManager, IUnitOfWork unitOfWork, IGenerateEmailVerificationPage Page, IAuthenticationService Service)
        {
            _userManager = userManager;
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
    }
}
