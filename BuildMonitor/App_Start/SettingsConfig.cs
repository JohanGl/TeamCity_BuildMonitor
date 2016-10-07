using BuildMonitor.Models.Home.Settings;
using System;
using System.IO;
using System.Xml.Serialization;

namespace BuildMonitor
{
	public static class SettingsConfig
	{
		public static void Initialize()
		{
			string path = AppDomain.CurrentDomain.BaseDirectory + "/App_Data/Settings.xml";
			using (var reader = new StreamReader(path))
			{
				var serializer = new XmlSerializer(typeof(Settings));
				Settings.Current = (Settings)serializer.Deserialize(reader);
			}			
		}
	}
}