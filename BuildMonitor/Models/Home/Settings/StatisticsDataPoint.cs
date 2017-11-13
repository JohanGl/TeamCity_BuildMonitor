using System.Xml.Serialization;

namespace BuildMonitor.Models.Home.Settings
{
	public class StatisticsDataPoint
	{
		[XmlAttribute("name")]
		public string Name { get; set; }

		[XmlAttribute("label")]
		public string Label { get; set; }
	}
}