using System;
using System.Collections.Generic;
using System.Configuration;
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
					var url = teamCityUrl + (string)buildJson.href;

					var buildStatusJsonString = RequestHelper.GetJson(url);
					var buildStatusJson = JsonConvert.DeserializeObject<dynamic>(buildStatusJsonString ?? string.Empty);

					runningBuilds.Add(buildId, buildStatusJson);
				}
			}
			catch
			{
			}
		}

		protected void UpdateBuildStatusFromRunningBuildJson(string buildId)
		{
			buildStatusJson = runningBuilds[buildId];
		}

		protected BuildStatus GetBuildStatusForRunningBuild(string buildId)
		{
			if (runningBuilds.ContainsKey(buildId))
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

		protected string[] GetRunningBuildBranchAndProgress(string buildId)
		{
			var result = new[]
            {
                string.Empty,
                string.Empty
            };

			try
			{
				result[0] = (string)runningBuilds[buildId].branchName ?? "default";

				var percentage = (string)runningBuilds[buildId].percentageComplete;
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
			const int second = 1;
			const int minute = 60 * second;
			const int hour = 60 * minute;
			const int day = 24 * hour;
			const int month = 30 * day;

			try
			{
				var dateTime = DateTime.ParseExact((string)buildStatusJson.startDate, "yyyyMMdd'T'HHmmsszzz", CultureInfo.InvariantCulture);

				var timeSpan = new TimeSpan(DateTime.Now.Ticks - dateTime.Ticks);
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
				return string.Empty;
			}
		}
	}
}