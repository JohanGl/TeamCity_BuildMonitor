namespace BuildMonitor.Models.Tests
{
	public class TestRunResult
	{
		public int BuildNumber { get; set; }

		public int PassedCount { get; set; }

		public int FailedCount { get; set; }

		public int IgnoredCount { get; set; }
	}
}