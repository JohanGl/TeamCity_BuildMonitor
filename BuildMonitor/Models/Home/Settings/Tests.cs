using System.Xml.Serialization;

namespace BuildMonitor.Models.Home.Settings
{
	public class Tests
	{
		[XmlAttribute("id")]
		public string Id { get; set; }

		[XmlAttribute("maxBuildCount")]
		public int MaxBuildCount { get; set; }

		[XmlAttribute("text")]
		public string Text { get; set; }
	}
}