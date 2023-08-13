namespace RichManClient.Pages
{
    public class SharedStateService
    {
        public string? Api_Uri { get; set; } = "https://bigrichman.azurewebsites.net/";
    }


    public class WorkerSelectionService
    {
        public event Action<string> OnWorkerSelected;

        public void SelectWorker(string workerName)
        {
            OnWorkerSelected?.Invoke(workerName);
        }
    }
}
