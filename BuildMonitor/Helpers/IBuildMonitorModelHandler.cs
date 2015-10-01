using BuildMonitor.Models.Home;

namespace BuildMonitor.Helpers
{
	public interface IBuildMonitorModelHandler
	{
		BuildMonitorViewModel GetModel();
	}
}