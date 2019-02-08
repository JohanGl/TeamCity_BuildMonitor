namespace BuildMonitor.Models
{
	/// <summary>
	/// Safe storage of app secrets in development in ASP.NET Core
	/// https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-2.2&tabs=windows
	/// </summary>
	public class SecretTeamCitySettings
	{
		public string UserName { get; set; }
		public string Password { get; set; }
	}
}