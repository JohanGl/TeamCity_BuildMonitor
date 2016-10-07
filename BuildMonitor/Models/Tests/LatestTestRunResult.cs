namespace BuildMonitor.Models.Tests
{
	public class LatestTestRunResult
	{
		public int PassedCount { get; set; }

		public int FailedCount { get; set; }

		public int IgnoredCount { get; set; }
	}
}