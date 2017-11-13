using System;
using System.IO;
using System.Xml.Serialization;
using AutomationSettings = BuildMonitor.Models.Automation.Settings.Settings;
using HomeSettings = BuildMonitor.Models.Home.Settings.Settings;

namespace BuildMonitor
{
	public static class SettingsConfig
	{
		public static void Initialize()
		{
			LoadAutomationConfig();
			LoadBuildConfig();
		}

		private static void LoadAutomationConfig()
		{
			string path = AppDomain.CurrentDomain.BaseDirectory + "/App_Data/Automation.config";
			using (StreamReader reader = new StreamReader(path))
			{
				XmlSerializer serializer = new XmlSerializer(typeof(AutomationSettings));
				AutomationSettings.Current = (AutomationSettings)serializer.Deserialize(reader);
			}
		}

		private static void LoadBuildConfig()
		{
			string path = AppDomain.CurrentDomain.BaseDirectory + "/App_Data/Settings.xml";
			using (StreamReader reader = new StreamReader(path))
			{
				XmlSerializer serializer = new XmlSerializer(typeof(HomeSettings));
				HomeSettings.Current = (HomeSettings)serializer.Deserialize(reader);
			}
		}
	}
}