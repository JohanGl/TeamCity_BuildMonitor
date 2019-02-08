using System.Collections.Generic;
using BuildMonitor.Helpers;
using BuildMonitor.Models;
using BuildMonitor.Models.Index;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace BuildMonitor.Pages
{
	public class IndexModel : PageModel
	{
		public List<Project> Projects { get; set; }

		private readonly IBuildMonitorModelHandler modelHandler;

		public IndexModel(IOptions<TeamCitySettings> config)
		{
			RequestHelper.Username = config.Value.UserName;
			RequestHelper.Password = config.Value.Password;

			modelHandler = new DefaultBuildMonitorModelHandler();
			//modelHandler = new CustomBuildMonitorModelHandler();
			modelHandler.Initialize(config.Value);

			var model = modelHandler.GetModel();
			Projects = model.Projects;
		}

		public void OnGet()
		{
		}
	}
}
