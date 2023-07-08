using Microsoft.AspNetCore.Identity;
using static ImageApp.Extensions.ServiceExtensions;

namespace ImageApp.Extensions
{
	public static class CustomIdentityBuilderExtensions
	{
		public static IdentityBuilder AddPasswordlessLoginTotpTokenProvider(this IdentityBuilder builder)
		{
			var userType = builder.UserType;
			var totpProvider = typeof(PasswordlessLoginTotpTokenProvider<>).MakeGenericType(userType);
			return builder.AddTokenProvider("PasswordlessLoginTotpProvider", totpProvider);
		}
	}
}
