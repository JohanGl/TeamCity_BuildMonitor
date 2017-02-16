using System.Collections.Generic;
using System.Xml.Serialization;

namespace BuildMonitor.Models.Automation.Settings
{
	public class Group
	{
		[XmlAttribute("name")]
		public string Name { get; set; }

		public List<Job> Jobs { get; set; }
	}
}