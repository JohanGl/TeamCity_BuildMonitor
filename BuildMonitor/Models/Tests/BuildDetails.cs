using System;

namespace BuildMonitor.Models.Tests
{
	public class BuildDetails
	{
		public string Number { get; set; }

		public int PassedCount { get; set; }

		public int FailedCount { get; set; }

		public int IgnoredCount { get; set; }

		public DateTime FinishDate { get; set; }
	}
}