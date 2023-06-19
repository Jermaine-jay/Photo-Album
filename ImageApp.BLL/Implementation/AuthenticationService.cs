using ImageApp.BLL.Interface;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageApp.BLL.Implementation
{
	public class AuthenticationService : IAuthenticationService
	{
		private readonly IConfiguration _configuration;
		private readonly string VerificationUrl = "https://api.zerobounce.net/v2/validate";
		private readonly HttpClient _httpClient;
		private string? _ApiKey;
		private string? _Url;

		public AuthenticationService(IConfiguration configuration)
        {
			_configuration = configuration;
			_ApiKey = _configuration.GetSection("ZeroBook")?.GetSection("ApiKey")?.Value;
			_Url = _configuration.GetSection("ZeroBook")?.GetSection("Url")?.Value;
			_httpClient = new HttpClient();
		}
        public Task<(bool successful, string msg)> ConfirmEmail(string userId, string code)
		{
			throw new NotImplementedException();
		}

		public Task Execute(string apiKey, string subject, string message, string email)
		{
			throw new NotImplementedException();
		}

		public Task<(bool successful, string msg)> SendEmailAsync(string email, string subject, string htmlMessage)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> VerifyEmail(string emailAddress)
		{
			try
			{
				using (var client = new HttpClient())
				{
					var parameters = $"api_key={_ApiKey}&email={emailAddress}";
					var response = await client.GetAsync($"{_Url}?{parameters}");
					response.EnsureSuccessStatusCode();

					var responseContent = await response.Content.ReadAsStringAsync();
					var getResponse = JsonConvert.DeserializeObject<ResolveBankResponse>(listResponse);
					if (responseContent.) { }
					return true; 
				}
			}
			catch (Exception ex)
			{
				// Handle any exceptions that occurred during the API call
				Console.WriteLine("Error verifying email: " + ex.Message);
				return false;
			}
			throw new NotImplementedException();
		}
	}
}
