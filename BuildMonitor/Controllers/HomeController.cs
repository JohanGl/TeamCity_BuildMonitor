using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using BuildMonitor.Helpers;
using BuildMonitor.Models.Home;
using BuildMonitor.Models.Home.Settings;
using BuildMonitor.Models.Statistics;
using BuildMonitor.Models.Tests;


namespace BuildMonitor.Controllers
{
	public class HomeController : Controller
	{
		private readonly IBuildMonitorModelHandler modelHandler;

		public HomeController()
		{
			// modelHandler = new DefaultBuildMonitorModelHandler();
			modelHandler = new CustomBuildMonitorModelHandler();

			RequestHelper.Username = ConfigurationManager.AppSettings["teamcity_username"];
			RequestHelper.Password = ConfigurationManager.AppSettings["teamcity_password"];
		}

		public ActionResult Index()
		{
			var model = modelHandler.GetModel();
			return View(model);
		}

		public JsonResult GetBuilds()
		{
			var model = modelHandler.GetModel();

			var builds = model.Projects.SelectMany(p => p.Builds).ToList();

			var result = new BuildsJson();

			foreach (var build in builds)
			{
				result.Builds.Add(new BuildJson()
				{
					Id = build.Id,
					Content = RenderPartialViewToString("_BuildItem", build)
				});
			}

			return Json(result, JsonRequestBehavior.AllowGet);

		}

		public JsonResult GetLastStatistics(string buildConfigurationId)
		{
			// Get the raw data.
			BuildStatistics latestStatistics = TestsHelper.GetLatestRunStatistics(buildConfigurationId);

			// Transform to the structure that is expected by the client.
			object[] statisticsResultResponse = new object[latestStatistics.Items.Count + 1];
			statisticsResultResponse[0] = new object[] {"Item", "Count"};
			int counter = 1;
			foreach (KeyValuePair<string, string> statisticsItem in latestStatistics.Items)
			{
				string label = String.Format(CultureInfo.InvariantCulture, "{0}: {1}", statisticsItem.Key, statisticsItem.Value);
				int value = Int32.Parse(statisticsItem.Value);
				statisticsResultResponse[counter] = new object[] {label, value};
				counter++;
			}

			// Transform the result to JSON.
			return Json(statisticsResultResponse, JsonRequestBehavior.AllowGet);
		}

		protected string RenderPartialViewToString(string viewName, object model)
		{
			if (string.IsNullOrEmpty(viewName))
			{
				viewName = ControllerContext.RouteData.GetRequiredString("action");
			}

			ViewData.Model = model;

			using (var stringWriter = new StringWriter())
			{
				ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
				ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, stringWriter);
				viewResult.View.Render(viewContext, stringWriter);

				return stringWriter.GetStringBuilder().ToString();
			}
		}
	}
}