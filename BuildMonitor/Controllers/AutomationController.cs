using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Web.Mvc;
using BuildMonitor.Helpers;
using BuildMonitor.Models.Automation.Settings;
using BuildMonitor.Models.Automation.ViewModels;
using BuildMonitor.Models.Tests;

namespace BuildMonitor.Controllers
{
	public class AutomationController : Controller
	{
		public ActionResult Index()
		{
			Dashboard dashboard = this.GetInitialViewModel();
			return this.View(dashboard);
		}

		[HttpGet]
		public JsonResult TestRunResults(string buildConfigurationId)
		{
			// Get the raw data.
			TestRunResult testRunResult = TestsHelper.GetLatestRunResult(buildConfigurationId);

			// Transform to the structure that is expected by the client.
			object[] testRunResultResponse =
			{
				new object[] { "Status", "Count" },
				new object[] { "Passed: " + testRunResult.PassedCount, testRunResult.PassedCount },
				new object[] { "Failed: " + testRunResult.FailedCount, testRunResult.FailedCount },
				new object[] { "Ignored: " + testRunResult.IgnoredCount, testRunResult.IgnoredCount }
			};

			TestRunResultsResponse response = new TestRunResultsResponse
			{
				UpdatedText = "Updated " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
				TestResults = testRunResultResponse
			};

			// Transform the result to JSON.
			return Json(response, JsonRequestBehavior.AllowGet);
		}

		private Dashboard GetInitialViewModel()
		{
			string teamCityUrl = ConfigurationManager.AppSettings["teamcity_api_url"];

			Dashboard dashboard = new Dashboard
			{
				Sections = new List<Section>()
			};

			foreach (Group group in Settings.Current.Groups)
			{
				Section section = new Section
				{
					Title = group.Name,
					Gauges = new List<Gauge>()
				};

				foreach (Job job in group.Jobs)
				{
					Gauge gauge = new Gauge
					{
						Title = job.Text,
						BuildConfigurationId = job.Id,
						DetailsUrl = String.Format(CultureInfo.InvariantCulture, "{0}/viewType.html?buildTypeId={1}", teamCityUrl, job.Id),
						Environment = job.Environment
					};
					section.Gauges.Add(gauge);
				}

				dashboard.Sections.Add(section);
			}

			return dashboard;
		}
	}
}