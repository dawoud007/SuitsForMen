namespace RichManClient.Pages
{
    public class SharedStateService
    {
        public string? Api_Uri { get; set; } = "http://localhost:5000/";
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
