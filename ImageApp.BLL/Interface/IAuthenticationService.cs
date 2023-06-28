using ImageApp.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ImageApp.BLL.Interface
{
	public interface IAuthenticationService
	{
		Task<bool> VerifyEmail(string emailAddress);
		Task<(bool successful, string msg)> ConfirmEmail(string userId, string code);
		Task<bool> Execute(string email, string subject, string htmlMessage);
		Task<(bool successful, string msg)> SendEmailAsync(string subject, string message, string email);
		Task<bool> RegistrationMail(IUrlHelper urlHelper, User newUser);

	}
}
