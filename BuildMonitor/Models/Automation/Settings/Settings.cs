using System.Collections.Generic;

namespace BuildMonitor.Models.Automation.Settings
{
	public class Settings
	{
		public static Settings Current { get; set; }

		public List<Group> Groups { get; set; }
	}
}