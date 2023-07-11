using ImageApp.BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace ImageApp.BLL.Interface
{
	public interface IRecoveryService
	{
		Task<(bool successful, string msg)> ForgotPassword(ForgotPasswordVM model);
		Task<(bool successful, string msg)> ResetPassword(ResetPasswordVM model);
		Task<(bool successful, string msg)> ChangeEmail(string userId, string code);
		Task<string> ResetEmailToken(string userId);
		Task<(bool successful, string msg)> ChangeDetailToken(string userId);
		Task<(bool successful, string msg)> VerifyChangeDetailToken(ConfirmTokenVM model);
		Task<(bool successful, string msg)> ChangePassword(ChangePasswordVM model);

	}
}
