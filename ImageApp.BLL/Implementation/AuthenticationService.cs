using ImageApp.BLL.Interface;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

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
				using (_httpClient)
				{
					var parameters = $"api_key={_ApiKey}&email={emailAddress}";
					var response = await _httpClient.GetAsync($"{_Url}?{parameters}");
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
	}
}
