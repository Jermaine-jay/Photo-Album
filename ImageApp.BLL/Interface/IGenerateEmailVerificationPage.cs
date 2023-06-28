namespace ImageApp.BLL.Interface
{
	public interface IGenerateEmailVerificationPage
	{
		public string EmailVerificationPage(string name, string callbackurl);
		public string PasswordResetPage(string callbackurl);
	}
}
