using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageApp.BLL.Interface
{
    public interface IAuthenticationService
    {
        Task<bool> VerifyEmail(string emailAddress);
        Task<(bool successful, string msg)> ConfirmEmail(string userId, string code);
        Task<(bool successful, string msg)> SendEmailAsync(string email, string subject, string htmlMessage);
        Task Execute(string apiKey, string subject, string message, string email);
    }
}
