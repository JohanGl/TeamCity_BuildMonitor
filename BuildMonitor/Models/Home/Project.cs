using System.Collections.Generic;

namespace BuildMonitor.Models.Home
{
	public class Project
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public List<Build> Builds { get; set; }

		public Project()
		{
			Builds = new List<Build>();
		}
	}
}