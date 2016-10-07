using System;
using System.Collections.Generic;
using System.Globalization;
using BuildMonitor.Models.Home.Settings;
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
		private static readonly string BuildUrl;

		static TestsHelper()
		{
			TestsHelper.TeamCityUrl = ConfigurationManager.AppSettings["teamcity_api_url"];
			TestsHelper.FinishedBuildsUrl = TestsHelper.TeamCityUrl + ConfigurationManager.AppSettings["teamcity_api_builds"] + "?locator=state:finished";
			TestsHelper.LatestFinishedBuildUrl = FinishedBuildsUrl + "&count=1";
			TestsHelper.BuildUrl = TestsHelper.TeamCityUrl + ConfigurationManager.AppSettings["teamcity_api_build"];
		}

		public static TestRunResult[] GetHistoryRunResults()
		{
			List<BuildSummary> buildSummaries = TestsHelper.GetFinishedBuilds(Settings.Current.Tests.Id);


			TestRunResult[] result = new TestRunResult[Settings.Current.Tests.MaxBuildCount];

			for (int i = 0; i < Settings.Current.Tests.MaxBuildCount; i++)
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
			BuildSummary latestBuildSummary = TestsHelper.GetLatestFinishedBuild(Settings.Current.Tests.Id);
			BuildDetails latestBuild = TestsHelper.GetBuildDetails(latestBuildSummary.Id);

			return new TestRunResult
			{
				PassedCount = latestBuild.PassedCount,
				FailedCount = latestBuild.FailedCount,
				IgnoredCount = latestBuild.IgnoredCount
			};
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
			BuildDetails cachedValue = BuildsCache.Get( buildId );
			if( cachedValue != null )
			{
				return cachedValue;
			}

			string url = String.Format(CultureInfo.InvariantCulture, TestsHelper.BuildUrl, buildId);
			string buildJsonString = RequestHelper.GetJson(url);
			dynamic buildJson = JsonConvert.DeserializeObject<dynamic>(buildJsonString);

			BuildDetails result = new BuildDetails
			{
				Number = (string)buildJson.number,
				PassedCount = buildJson.testOccurrences.passed != null ? (int)buildJson.testOccurrences.passed : 0,
				FailedCount = buildJson.testOccurrences.failed != null ? (int)buildJson.testOccurrences.failed : 0,
				IgnoredCount = buildJson.testOccurrences.ignored != null ? (int)buildJson.testOccurrences.ignored : 0
			};

			BuildsCache.Add( buildId, result );

			return result;
		}
	}
}