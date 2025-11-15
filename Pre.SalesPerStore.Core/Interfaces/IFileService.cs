using Pre.SalesPerStore.Core.Entities;

namespace Pre.SalesPerStore.Core.Interfaces;

public interface IFileService
{
    List<Store> LoadStoresFromFile(string fileName);
}
