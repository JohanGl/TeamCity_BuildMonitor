using System;
using System.Globalization;
using System.Web;
using BuildMonitor.Models.Statistics;

namespace BuildMonitor.Helpers
{
	public static class BuildStatisticsCache
	{
		public static BuildStatistics Get(int buildId)
		{
			string key = BuildStatisticsCache.GetKey(buildId);
			object rawValue = HttpContext.Current.Cache[ key ];
			return rawValue == null
				? null
				: (BuildStatistics) rawValue;
		}

		public static void Add(int buildId, BuildStatistics buildStatistics)
		{
			string key = BuildStatisticsCache.GetKey(buildId);
			HttpContext.Current.Cache.Insert( key, buildStatistics );
		}

		private static string GetKey(int buildId)
		{
			return String.Format( CultureInfo.InvariantCulture, "BuildStatistics-{0}", buildId );
		}
	}
}