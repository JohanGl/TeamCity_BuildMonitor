using System.Collections.Generic;
using System.Xml.Serialization;

namespace BuildMonitor.Models.Home.Settings
{
	public class StatisticsPie
	{
		[XmlAttribute("name")]
		public string Name { get; set; }

		[XmlAttribute("buildConfigurationId")]
		public string BuildConfigurationId { get; set; }

		public List<StatisticsDataPoint> StatisticsDataPoints { get; set; }
	}
}