using System.Collections.Generic;

namespace BuildMonitor.Models.Home
{
	public class BuildMonitorViewModel
	{
		public List<Project> Projects { get; set; }

		public BuildMonitorViewModel()
		{
			Projects = new List<Project>();
		}
	}
}