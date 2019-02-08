using System.Collections.Generic;

namespace BuildMonitor.Models.Index
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