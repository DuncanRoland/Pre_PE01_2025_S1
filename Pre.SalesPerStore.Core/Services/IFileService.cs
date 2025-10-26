using System;

namespace Pre.SalesPerStore.Core.Services;

public interface IFileService
{
    List<Store> LoadStoresFromFile(string fileName);
}
