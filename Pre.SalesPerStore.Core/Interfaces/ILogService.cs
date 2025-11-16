using Pre.SalesPerStore.Core.Events;

namespace Pre.SalesPerStore.Core.Interfaces;

public interface ILogService
{
    public void Log(object? sender, PrintEventArgs e);
}