using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using BuildMonitor.Models.Home;
using Newtonsoft.Json;

namespace BuildMonitor.Helpers
{
	public abstract class BuildMonitorModelHandlerBase : IBuildMonitorModelHandler
	{
		protected readonly string teamCityUrl;
		protected readonly string projectsUrl;
		protected readonly string buildTypesUrl;
		protected readonly string runningBuildsUrl;
		protected readonly string buildStatusUrl;
		protected readonly string buildQueueUrl;
		protected Dictionary<string, dynamic> runningBuilds;

		protected dynamic projectsJson;
		protected dynamic buildTypesJson;
		protected dynamic buildQueueJson;
		protected dynamic buildStatusJson;

		protected BuildMonitorModelHandlerBase()
		{
			teamCityUrl = ConfigurationManager.AppSettings["teamcity_api_url"];
			projectsUrl = teamCityUrl + ConfigurationManager.AppSettings["teamcity_api_projects"];
			buildTypesUrl = teamCityUrl + ConfigurationManager.AppSettings["teamcity_api_buildtypes"];
			runningBuildsUrl = teamCityUrl + ConfigurationManager.AppSettings["teamcity_api_runningbuilds"];
			buildStatusUrl = teamCityUrl + ConfigurationManager.AppSettings["teamcity_api_buildstatus"];
			buildQueueUrl = teamCityUrl + ConfigurationManager.AppSettings["teamcity_api_buildqueue"];
		}

		protected void GetTeamCityBuildsJson()
		{
			var projectsJsonString = RequestHelper.GetJson(projectsUrl);
			projectsJson = JsonConvert.DeserializeObject<dynamic>(projectsJsonString);

			var buildTypesJsonString = RequestHelper.GetJson(buildTypesUrl);
			buildTypesJson = JsonConvert.DeserializeObject<dynamic>(buildTypesJsonString);

			var buildQueueJsonString = RequestHelper.GetJson(buildQueueUrl);
			buildQueueJson = buildQueueJsonString != null ? JsonConvert.DeserializeObject<dynamic>(buildQueueJsonString) : null;

			UpdateRunningBuilds();
		}

		private void UpdateRunningBuilds()
		{
			try
			{
				runningBuilds = new Dictionary<string, dynamic>();

				var runningBuildsJsonString = RequestHelper.GetJson(runningBuildsUrl);
				var runningBuildsJson = runningBuildsJsonString != null ? JsonConvert.DeserializeObject<dynamic>(runningBuildsJsonString) : null;

				var count = (int)runningBuildsJson.count;
				for (int i = 0; i < count; i++)
				{
					var buildJson = runningBuildsJson.build[i];

					var buildId = (string)buildJson.buildTypeId;
					var branchName = (string) buildJson.branchName;
					var url = teamCityUrl + (string)buildJson.href;

					var buildStatusJsonString = RequestHelper.GetJson(url);
					var buildStatusJson = JsonConvert.DeserializeObject<dynamic>(buildStatusJsonString ?? string.Empty);
					
					runningBuilds.Add(buildId + branchName, buildStatusJson);
					if (!runningBuilds.ContainsKey(buildId))
					{
						runningBuilds.Add(buildId, buildStatusJson);    
					}
				}
			}
			catch(Exception ex)
			{
				Debug.Write(ex);
			}
		}

		protected void UpdateBuildStatusFromRunningBuildJson(string buildId, string branchName = null)
		{
			buildStatusJson = runningBuilds[buildId + branchName];
		}

		protected BuildStatus GetBuildStatusForRunningBuild(string buildId, string branchName = null)
		{
			if (runningBuilds.ContainsKey(buildId + branchName))
			{
				return BuildStatus.Running;
			}

			if (buildStatusJson == null)
			{
				return BuildStatus.None;
			}

			switch ((string)buildStatusJson.status)
			{
				case "SUCCESS":
					return BuildStatus.Success;

				case "FAILURE":
					return BuildStatus.Failure;

				case "ERROR":
					return BuildStatus.Error;

				default:
					return BuildStatus.None;
			}
		}

		protected string[] GetRunningBuildBranchAndProgress(string buildId, string branchName = null)
		{
			var runningBuild = runningBuilds[buildId + branchName];

			var result = new[]
			{
				string.Empty,
				string.Empty
			};

			try
			{
				result[0] = (string)runningBuild.branchName ?? "default";

				var percentage = (string)runningBuild.percentageComplete;
				result[1] = !string.IsNullOrWhiteSpace(percentage) ? percentage + "%" : "0%";
			}
			catch
			{
			}

			return result;
		}

		public abstract BuildMonitorViewModel GetModel();

		protected string GetLastRunText()
		{
			DateTime dateTime = DateTime.ParseExact((string)buildStatusJson.startDate, "yyyyMMdd'T'HHmmsszzz", CultureInfo.InvariantCulture);
			TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks - dateTime.Ticks);
			return TestsHelper.FormatTimestamp(timeSpan);
		}
	}
}