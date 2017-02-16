using System.Collections.Generic;

namespace BuildMonitor.Models.Automation.ViewModels
{
	public class Section
	{
		public string Title { get; set; }

		public List<Gauge> Gauges { get; set; }
	}
}