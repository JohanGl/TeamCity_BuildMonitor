using System.Xml.Serialization;

namespace BuildMonitor.Models.Home.Settings
{
    public class LastUpdateWarning
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("text")]
        public string Text { get; set; }
    }
}