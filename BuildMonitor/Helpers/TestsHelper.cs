using BuildMonitor.Models.Tests;

namespace BuildMonitor.Helpers
{
	public static class TestsHelper
	{
		public static TestRunResult[] GetHistoryRunResults()
		{
			// TODO: Dummy data
			TestRunResult[] result = new TestRunResult[100];

			for (int i = 0; i < 100; i++)
			{
				result[i] = new TestRunResult
				{
					BuildNumber = i,
					PassedCount = 2*i,
					FailedCount = 3*i,
					IgnoredCount = 2*i
				};
			}

			return result;
		}

		public static TestRunResult GetLatestRunResult()
		{
			// TODO: Dummy data!
			return new TestRunResult
			{
				PassedCount = 100,
				FailedCount = 3,
				IgnoredCount = 61
			};
		}
	}
}