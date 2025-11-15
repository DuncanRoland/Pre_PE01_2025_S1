using System;
using Pre.SalesPerStore.Core.Entities;

namespace Pre.SalesPerStore.Core.Services;

public interface IFileService
{
    List<Store> LoadStoresFromFile(string fileName);
}
