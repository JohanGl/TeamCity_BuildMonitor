namespace BuildMonitor.Models
{
	/// <summary>
	/// These are loaded from appsettings.json. Note that UserName and Password can be overridden from a user secrets file.
	/// See SecretTeamCitySettings.cs for more information of how to use encrypted/hidden credentials.
	/// </summary>
	public class TeamCitySettings
	{
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Url { get; set; }
		public string Projects { get; set; }
		public string BuildTypes { get; set; }
		public string BuildStatus { get; set; }
		public string RunningBuilds { get; set; }
		public string BuildQueue { get; set; }
	}
}