using System.Xml.Serialization;

namespace BuildMonitor.Models.Automation.Settings
{
	public class Job
	{
		[XmlAttribute("id")]
		public string Id { get; set; }

		[XmlAttribute("text")]
		public string Text { get; set; }

		[XmlAttribute("branch")]
		public string Branch { get; set; }

		[XmlAttribute("environment")]
		public string Environment { get; set; }
	}
}