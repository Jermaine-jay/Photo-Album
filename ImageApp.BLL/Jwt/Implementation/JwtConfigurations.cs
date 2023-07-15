using ImageApp.BLL.Jwt.Interface;
using ImageApp.DAL.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace ImageApp.BLL.Jwt.Implementation
{
    public class JwtConfigurations : IJwtConfigurations
	{
		private readonly IConfiguration _configuration;
		private string _apiKey;
		private string  _issuer;
		private string  _audience;
     
		public JwtConfigurations(IConfiguration configuration)
        {
			_configuration = configuration;
			_apiKey = _configuration["Jwt:Key"];
			_audience = _configuration["Jwt:Audience"];
			_issuer = _configuration["Jwt:Issuer"];

		}
        public string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_apiKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
            };
            var token = new JwtSecurityToken(_issuer,
				_audience,
                claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

		public bool IsTokenValid(string token)
		{
			var mySecret = Encoding.UTF8.GetBytes(_apiKey);
			var mySecurityKey = new SymmetricSecurityKey(mySecret);
			var tokenHandler = new JwtSecurityTokenHandler();
			try
			{
				tokenHandler.ValidateToken(token,
				new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidIssuer = _issuer,
					ValidAudience = _audience,
					IssuerSigningKey = mySecurityKey,
				}, out SecurityToken validatedToken);
			}
			catch
			{
				return false;
			}
			return true;
		}
	}
}

