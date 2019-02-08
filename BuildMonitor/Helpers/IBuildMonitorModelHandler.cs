using BuildMonitor.Models;
using BuildMonitor.Models.Index;

namespace BuildMonitor.Helpers
{
	public interface IBuildMonitorModelHandler
	{
		void Initialize(TeamCitySettings settings);
		BuildMonitorViewModel GetModel();
	}
}