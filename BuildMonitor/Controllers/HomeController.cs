using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using BuildMonitor.Helpers;
using BuildMonitor.Models.Home;


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