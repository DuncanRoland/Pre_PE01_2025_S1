using Pre.SalesPerStore.Core.Events;
using Pre.SalesPerStore.Core.Interfaces;

namespace Pre.SalesPerStore.Core.Services;

public class LogService : ILogService
{
    public void Log(object? sender, PrintEventArgs e)
    {
        Console.WriteLine(e.ToString());
    }
}