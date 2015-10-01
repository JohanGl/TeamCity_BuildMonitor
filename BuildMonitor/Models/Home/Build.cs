namespace BuildMonitor.Models.Home
{
	public class Build
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public BuildStatus Status { get; set; }
		public string Branch { get; set; }
		public string Progress { get; set; }
		public string UpdatedBy { get; set; }
		public string LastRunText { get; set; }
		public bool IsQueued { get; set; }

		public string StatusText
		{
			get
			{
				switch (Status)
				{
					case BuildStatus.Success:
						return "OK";

					case BuildStatus.Failure:
						return "FAILED";

					case BuildStatus.Running:
						return "RUNNING";

					case BuildStatus.Error:
						return "ERROR";

					default:
						return "UNKNOWN";
				}
			}
		}
	}
}