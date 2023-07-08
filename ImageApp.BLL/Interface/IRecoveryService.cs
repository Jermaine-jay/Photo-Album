using ImageApp.BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace ImageApp.BLL.Interface
{
	public interface IRecoveryService
	{
		Task<(bool successful, string msg)> ForgotPassword(IUrlHelper urlHelper, ForgotPasswordVM model);
		Task<(bool successful, string msg)> ResetPassword(ResetPasswordVM model);
		Task<(bool successful, string msg)> ChangeEmail(string userId, string code);
		Task<(bool successful, string msg)> ResetEmail(string userId, string code);
		Task<(bool successful, string msg)> ChangeDetailsToken(IUrlHelper urlHelper, string userId);
		Task<(bool successful, string msg)> VerifyChangePasswordToken(ConfirmTokenVM model);
		Task<(bool successful, string msg)> ChangePassword(ChangePasswordVM model);

	}
}
