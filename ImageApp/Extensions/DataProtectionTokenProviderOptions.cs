namespace ImageApp.Extensions
{
	public class DataProtectionTokenProviderOptions
	{
		public string Name { get; set; } = "DataProtectorTokenProvider";
		public TimeSpan TokenLifespan { get; set; } = TimeSpan.FromDays(1);
	}
}
