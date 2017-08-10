namespace BuildMonitor.Models.LastUpdateWarning.ViewModels
{
    public class LastUpdateWarningResponse
    {
        public bool IsOverdue { get; set; }

        public string Message { get; set; }
    }
}