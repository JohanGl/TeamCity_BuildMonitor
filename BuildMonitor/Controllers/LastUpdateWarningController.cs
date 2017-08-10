using System;
using System.Globalization;
using System.Web.Mvc;
using BuildMonitor.Helpers;
using BuildMonitor.Models.Home.Settings;
using BuildMonitor.Models.LastUpdateWarning.ViewModels;

namespace BuildMonitor.Controllers
{
    public class LastUpdateWarningController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            const int limitInSeconds = 60 * 60 * 25; // 25 hours

            // Get the build configuration ID from the configuration file.
            string buildConfigurationId = Settings.Current.LastUpdateWarning.Id;

            // Get the date of the last successful build.
            DateTime finishDate = TestsHelper.GetLatestSuccessfulBuildFinishDate(buildConfigurationId);

            // Check whether the last successful build completed within the expected time range.
            TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks - finishDate.Ticks);
            double deltaInSeconds = Math.Abs(timeSpan.TotalSeconds);
            bool isOverdue = deltaInSeconds > limitInSeconds; 
            
            // Build the warning message.
            string buildConfigurationName = Settings.Current.LastUpdateWarning.Text;
            string timestamp = TestsHelper.FormatTimestamp(timeSpan);
            string message = String.Format(CultureInfo.InvariantCulture, "The {0} was last updated {1}!", buildConfigurationName, timestamp);

            // Build the response.
            LastUpdateWarningResponse response = new LastUpdateWarningResponse
            {
                IsOverdue = isOverdue,
                Message = isOverdue ? message : "OK"
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}