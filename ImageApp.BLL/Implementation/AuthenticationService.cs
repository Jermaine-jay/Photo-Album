using ImageApp.BLL.Extensions;
using ImageApp.BLL.Interface;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using User = ImageApp.DAL.Entities.User;


namespace ImageApp.BLL.Implementation
{
	public class AuthenticationService : IAuthenticationService
	{
		private readonly EmailSenderOptions _emailSenderOptions;
		private readonly UserManager<User> _userManager;
		private readonly LinkGenerator _linkGenerator;
		private readonly IConfiguration _configuration;
		private readonly IGenerateEmailVerificationPage _generateEmailVerificationPage;
		private readonly IHttpContextAccessor _accessor;
		private string? _ApiKey;
		private string? _Url;

		public AuthenticationService(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, UserManager<User> userManager, IOptions<EmailSenderOptions> optionsAccessor, IGenerateEmailVerificationPage Page)
		{
			_linkGenerator = linkGenerator;
			_accessor = httpContextAccessor;
			_userManager = userManager;
			_configuration = configuration;
			_emailSenderOptions = optionsAccessor.Value;
			_ApiKey = _configuration["ZeroBook:ApiKey"];
			_Url = _configuration["ZeroBook:Url"];
			_generateEmailVerificationPage = Page;
		}


		public async Task<(bool successful, string msg)> ConfirmEmail(string userId, string code)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				return (false, "Error");
			}

			var result = await _userManager.ConfirmEmailAsync(user, code);
			if (result.Succeeded)
			{
				return (true, "EmailConfirmed");
			}
			return (false, "Invalid Verification Code");
		}


		public async Task<(bool successful, string msg)> SendEmailAsync(string email, string subject, string message)
		{
			if (string.IsNullOrEmpty(_emailSenderOptions.Password))
			{
				return (false, "Null SendGridKey");
			}
			await Execute(email, subject, message);
			return (true, "Verification Mail sent to your Email Address");
		}


		public async Task<bool> Execute(string email, string subject, string htmlMessage)
		{
			var message = new MimeMessage();
			message.From.Add(new MailboxAddress("Image App", _emailSenderOptions.Username));
			message.To.Add(new MailboxAddress(email, email));
			message.Subject = subject;

			var bodyBuilder = new BodyBuilder();
			bodyBuilder.HtmlBody = htmlMessage;
			message.Body = bodyBuilder.ToMessageBody();

			using (var client = new SmtpClient())
			{

				client.Connect(_emailSenderOptions.SmtpServer, _emailSenderOptions.Port, true);
				client.Authenticate(_emailSenderOptions.Email, _emailSenderOptions.Password);
				client.Send(message);
				client.Disconnect(true);

			}

			return true;
		}


		public async Task<bool> VerifyEmail(string emailAddress)
		{
			try
			{
				using (var httpClient = new HttpClient())
				{
					var parameters = $"api_key={_ApiKey}&email={emailAddress}";
					var response = await httpClient.GetAsync($"{_Url}?{parameters}");
					response.EnsureSuccessStatusCode();

					var responseContent = await response.Content.ReadAsStringAsync();
					var getResponse = JsonConvert.DeserializeObject<dynamic>(responseContent).status;
					if (getResponse == "valid")
					{
						return true;
					}
					return false;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error verifying email: " + ex.Message);
				return false;
			}
		}


		public async Task<bool> RegistrationMail(User newUser)
		{
			var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
			var callbackUrl = _linkGenerator.GetUriByAction(_accessor.HttpContext, "ConfirmEmail", "User", new { userId = newUser.Id, code });
			_ = await SendEmailAsync(newUser.Email, "Confirm your email",
				_generateEmailVerificationPage.EmailVerificationPage(newUser.UserName, callbackUrl));

			return true;
		}
	}
}
