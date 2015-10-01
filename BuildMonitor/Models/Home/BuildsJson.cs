using System;
using System.Collections.Generic;

namespace BuildMonitor.Models.Home
{
	public class BuildsJson
	{
		public string UpdatedText { get; set; }
		public List<BuildJson> Builds { get; set; }

		public BuildsJson()
		{
			UpdatedText = "Updated " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			Builds = new List<BuildJson>();
		}
	}
}