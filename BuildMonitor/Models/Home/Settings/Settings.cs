using System.Collections.Generic;

namespace BuildMonitor.Models.Home.Settings
{
	public class Settings
	{
		public static Settings Current { get; set; }

		public List<Group> Groups { get; set; }

		public Tests Tests { get; set; }

	    public LastUpdateWarning LastUpdateWarning { get; set; }
	}
}