namespace Pre.SalesPerStore.Core.Events;

public class PrintEventArgs : EventArgs
{
    private DateTime Timestamp { get; }
    private string ActionName { get; }
    private string Status { get; }
    private string? ErrorMessage { get; }

    public PrintEventArgs(string actionName, string status, string? errorMessage = null)
    {
        Timestamp = DateTime.Now;
        ActionName = actionName;
        Status = status;
        ErrorMessage = errorMessage;
    }

    public override string ToString()
        => $"[{Timestamp:yyyy-MM-dd HH:mm:ss}] - [{ActionName}] - [{Status}] - [{ErrorMessage ?? string.Empty}]";
}