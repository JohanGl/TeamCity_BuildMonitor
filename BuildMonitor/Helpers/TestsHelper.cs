using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.WebPages.Scope;
using BuildMonitor.Models.Home;
using BuildMonitor.Models.Home.Settings;
using BuildMonitor.Models.Statistics;
using BuildMonitor.Models.Tests;
using System.Configuration;
using Newtonsoft.Json;

namespace BuildMonitor.Helpers
{
	public static class TestsHelper
	{
		private static readonly string TeamCityUrl;
		private static readonly string FinishedBuildsUrl;
		private static readonly string LatestFinishedBuildUrl;
		private static readonly string LatestSuccessfulBuildUrl;
		private static readonly string BuildUrl;
		private static readonly string BuildStatisticsUrl;

		static TestsHelper()
		{
			TestsHelper.TeamCityUrl = ConfigurationManager.AppSettings["teamcity_api_url"];
			TestsHelper.FinishedBuildsUrl = TestsHelper.TeamCityUrl + ConfigurationManager.AppSettings["teamcity_api_builds"] + "?locator=state:finished";
			TestsHelper.LatestFinishedBuildUrl = FinishedBuildsUrl + "&count=1";
			TestsHelper.LatestSuccessfulBuildUrl = TestsHelper.TeamCityUrl + ConfigurationManager.AppSettings["teamcity_api_builds"] + "?locator=status:success&count=1";
			TestsHelper.BuildUrl = TestsHelper.TeamCityUrl + ConfigurationManager.AppSettings["teamcity_api_build"];
			TestsHelper.BuildStatisticsUrl = TestsHelper.TeamCityUrl + ConfigurationManager.AppSettings["teamcity_api_build_statistics"];
		}

		public static TestRunResult[] GetHistoryRunResults()
		{
			List<BuildSummary> buildSummaries = TestsHelper.GetFinishedBuilds(Settings.Current.Tests.Id);
			int buildSummaryCount = Math.Min(buildSummaries.Count, Settings.Current.Tests.MaxBuildCount);

			TestRunResult[] result = new TestRunResult[buildSummaryCount];

			for (int i = 0; i < buildSummaryCount; i++)
			{
				BuildDetails buildDetails = TestsHelper.GetBuildDetails(buildSummaries[i].Id);

				result[i] = new TestRunResult
				{
					BuildNumber = buildDetails.Number,
					PassedCount = buildDetails.PassedCount,
					FailedCount = buildDetails.FailedCount,
					IgnoredCount = buildDetails.IgnoredCount
				};
			}

			return result;
		}


		public static TestRunResult GetLatestRunResult()
		{
			return TestsHelper.GetLatestRunResult(Settings.Current.Tests.Id);
		}


		public static TestRunResult GetLatestRunResult(string buildConfigurationId)
		{
			BuildSummary latestBuildSummary = TestsHelper.GetLatestFinishedBuild(buildConfigurationId);
			BuildDetails latestBuild = TestsHelper.GetBuildDetails(latestBuildSummary.Id);

			return new TestRunResult
			{
				PassedCount = latestBuild.PassedCount,
				FailedCount = latestBuild.FailedCount,
				IgnoredCount = latestBuild.IgnoredCount
			};
		}

		public static BuildStatistics GetLatestRunStatistics(string buildConfigurationId)
		{
			BuildSummary latestBuildSummary = TestsHelper.GetLatestFinishedBuild(buildConfigurationId);
			return TestsHelper.GetBuildStatistics(latestBuildSummary.Id);
		}


		public static DateTime GetLatestSuccessfulBuildFinishDate(string buildConfigurationId)
		{
			BuildSummary buildSummary = TestsHelper.GetLatestSuccessfulBuild(buildConfigurationId);
			BuildDetails buildDetails = TestsHelper.GetBuildDetails(buildSummary.Id);
			return buildDetails.FinishDate;
		}


		public static string FormatTimestamp(TimeSpan timeSpan)
		{
			const int second = 1;
			const int minute = 60 * second;
			const int hour = 60 * minute;
			const int day = 24 * hour;
			const int month = 30 * day;

			try
			{
				double delta = Math.Abs(timeSpan.TotalSeconds);
				if (delta < 1 * minute)
				{
					return timeSpan.Seconds == 1 ? "one second ago" : timeSpan.Seconds + " seconds ago";
				}
				if (delta < 2 * minute)
				{
					return "a minute ago";
				}
				if (delta < 45 * minute)
				{
					return timeSpan.Minutes + " minutes ago";
				}
				if (delta < 90 * minute)
				{
					return "an hour ago";
				}
				if (delta < 24 * hour)
				{
					return timeSpan.Hours + " hours ago";
				}
				if (delta < 48 * hour)
				{
					return "yesterday";
				}
				if (delta < 30 * day)
				{
					return timeSpan.Days + " days ago";
				}

				if (delta < 12 * month)
				{
					int months = Convert.ToInt32(Math.Floor((double)timeSpan.Days / 30));
					return months <= 1 ? "one month ago" : months + " months ago";
				}
				else
				{
					int years = Convert.ToInt32(Math.Floor((double)timeSpan.Days / 365));
					return years <= 1 ? "one year ago" : years + " years ago";
				}
			}
			catch
			{
				return String.Empty;
			}
		}


		private static List<BuildSummary> GetFinishedBuilds(string buildConfigurationId)
		{
			string url = String.Format(CultureInfo.InvariantCulture, TestsHelper.FinishedBuildsUrl, buildConfigurationId);
			string buildsJsonString = RequestHelper.GetJson(url);
			dynamic buildsJson = JsonConvert.DeserializeObject<dynamic>(buildsJsonString);

			List<BuildSummary> result = new List<BuildSummary>();

			foreach (dynamic buildJson in buildsJson.build)
			{
				BuildSummary build = new BuildSummary
				{
					Id = (int)buildJson.id,
					Number = (string)buildJson.number,
					Href = (string)buildJson.href
				};

				result.Add(build);
			}

			return result;
		}


		private static BuildSummary GetLatestFinishedBuild(string buildConfigurationId)
		{
			string url = String.Format(CultureInfo.InvariantCulture, TestsHelper.LatestFinishedBuildUrl, buildConfigurationId);
			return TestsHelper.GetBuild(url);
		}


		private static BuildSummary GetLatestSuccessfulBuild(string buildConfigurationId)
		{
			string url = String.Format(CultureInfo.InvariantCulture, TestsHelper.LatestSuccessfulBuildUrl, buildConfigurationId);
			return TestsHelper.GetBuild(url);
		}


		private static BuildSummary GetBuild(string url)
		{
			string buildsJsonString = RequestHelper.GetJson(url);
			dynamic buildsJson = JsonConvert.DeserializeObject<dynamic>(buildsJsonString);

			dynamic buildJson = buildsJson.build[0];

			BuildSummary result = new BuildSummary
			{
				Id = (int)buildJson.id,
				Number = (string)buildJson.number,
				Href = (string)buildJson.href
			};

			return result;
		}


		private static BuildDetails GetBuildDetails(int buildId)
		{
			BuildDetails cachedValue = BuildsCache.Get(buildId);
			if (cachedValue != null)
			{
				return cachedValue;
			}

			string url = String.Format(CultureInfo.InvariantCulture, TestsHelper.BuildUrl, buildId);
			string buildJsonString = RequestHelper.GetJson(url);
			dynamic buildJson = JsonConvert.DeserializeObject<dynamic>(buildJsonString);

			dynamic testResults = buildJson.testOccurrences;

			BuildDetails result = new BuildDetails
			{
				Number = (string)buildJson.number,
				PassedCount = testResults != null && testResults.passed != null ? (int)testResults.passed : 0,
				FailedCount = testResults != null && testResults.failed != null ? (int)testResults.failed : 0,
				IgnoredCount = testResults != null && testResults.ignored != null ? (int)testResults.ignored : 0
			};

			if (buildJson.finishDate != null)
			{
				DateTime finishDate;
				if (DateTime.TryParseExact((string)buildJson.finishDate, "yyyyMMdd'T'HHmmsszzz", CultureInfo.InvariantCulture, DateTimeStyles.None, out finishDate))
				{
					result.FinishDate = finishDate;
				}
			}

			BuildsCache.Add(buildId, result);

			return result;
		}

		private static BuildStatistics GetBuildStatistics(int buildId)
		{
			List<StatisticsDataPoint> dataPoints = Settings.Current.StatisticsPie.StatisticsDataPoints;
			string[] monitoredPropertyNames = dataPoints.Select(p => p.Name).ToArray();

			BuildStatistics cachedValue = BuildStatisticsCache.Get(buildId);
			if (cachedValue != null)
			{
				return cachedValue;
			}

			string url = String.Format(CultureInfo.InvariantCulture, TestsHelper.BuildStatisticsUrl, buildId);
			string statisticsJsonString = RequestHelper.GetJson(url);
			dynamic statisticsJson = JsonConvert.DeserializeObject<dynamic>(statisticsJsonString);

			BuildStatistics result = new BuildStatistics
			{
				Items = new Dictionary<string, string>()
			};

			foreach (dynamic prop in statisticsJson.property)
			{
				string propertyName = prop.name.ToString();
				string propertyValue = prop.value.ToString();

				if (Array.IndexOf(monitoredPropertyNames, propertyName) > -1)
				{
					string label = Settings.Current.StatisticsPie.StatisticsDataPoints.Find(p => p.Name == propertyName).Label;
					result.Items.Add(label, propertyValue);
				}
			}

			BuildStatisticsCache.Add(buildId, result);

			return result;
		}
	}
}