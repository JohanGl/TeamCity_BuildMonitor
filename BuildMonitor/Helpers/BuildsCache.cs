using System;
using System.Globalization;
using System.Web;
using BuildMonitor.Models.Tests;

namespace BuildMonitor.Helpers
{
	public static class BuildsCache
	{
		public static BuildDetails Get(int buildId)
		{
			string key = BuildsCache.GetKey( buildId );
			object rawValue = HttpContext.Current.Cache[ key ];
			return rawValue == null
				? null
				: (BuildDetails) rawValue;
		}

		public static void Add(int buildId, BuildDetails build)
		{
			string key = BuildsCache.GetKey( buildId );
			HttpContext.Current.Cache.Insert( key, build );
		}

		private static string GetKey(int buildId)
		{
			return String.Format( CultureInfo.InvariantCulture, "BuildDetails-{0}", buildId );
		}
	}
}