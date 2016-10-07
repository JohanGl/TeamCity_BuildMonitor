using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BuildMonitor.Helpers;
using BuildMonitor.Models.Tests;

namespace BuildMonitor.Controllers
{
	public class TestsController : Controller
	{
		[HttpGet]
		public JsonResult History()
		{
			// Get the raw data.
			TestRunResult[] historyResults = TestsHelper.GetHistoryRunResults();

			// Transform to the structure that is expected by the client.
			var historyResultResponse = new object[historyResults.Length + 1];
			historyResultResponse[0] = new object[] { "Builds", "Ignored", "Failed", "Passed" };

			// NOTE: The chart expects the results in reverse order.
			for (int i = 0; i < historyResults.Length; i++)
			{
				TestRunResult currentResult = historyResults[i];
				historyResultResponse[historyResults.Length - i] = new object[]
				{
					currentResult.BuildNumber,
					currentResult.IgnoredCount,
					currentResult.FailedCount,
					currentResult.PassedCount
				};
			}

			// Transform the result to JSON.
			return Json(historyResultResponse, JsonRequestBehavior.AllowGet);
		}


		[HttpGet]
		public JsonResult Latest()
		{
			// Get the raw data.
			TestRunResult latestResult = TestsHelper.GetLatestRunResult();

			// Transform to the structure that is expected by the client.
			object[] latestResultResponse =
			{
				new object[] { "Status", "Count" },
				new object[] { "Passed: " + latestResult.PassedCount, latestResult.PassedCount },
				new object[] { "Failed: " + latestResult.FailedCount, latestResult.FailedCount },
				new object[] { "Ignored: " + latestResult.IgnoredCount, latestResult.IgnoredCount }
			};

			// Transform the result to JSON.
			return Json(latestResultResponse, JsonRequestBehavior.AllowGet);
		}
	}
}