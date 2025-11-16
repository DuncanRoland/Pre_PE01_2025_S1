using Pre.SalesPerStore.Core.Entities;
using Pre.SalesPerStore.Core.Interfaces;

namespace Pre.SalesPerStore.Core.Services;

public class StoreService : IStoreService
{
    readonly List<Store> _stores;

    public StoreService(IFileService fileService, string assetsFolderPath, string fileName = "stores_products.csv")
    {
        if (fileService == null) throw new ArgumentNullException(nameof(fileService));
        if (string.IsNullOrWhiteSpace(assetsFolderPath))
            throw new ArgumentException("Path empty", nameof(assetsFolderPath));

        var filePath = Path.Combine(assetsFolderPath, fileName);
        _stores = File.Exists(filePath)
            ? fileService.LoadStoresFromFile(filePath)
            : new List<Store>();
    }

    public IEnumerable<string> GetStoresByProduct(string productName)
    {
        return _stores
            .Where(store => store.Products.Any(product =>
                string.Equals(product.ProductName, productName, StringComparison.OrdinalIgnoreCase)))
            .Select(store => store.StoreName)
            .Distinct();
    }

    public IEnumerable<string> GetAllCountries()
    {
        return _stores
            .Select(store => store.StoreCountry)
            .Distinct();
    }

    public IEnumerable<string> GetAllStores()
    {
        return _stores.Select(store => store.StoreName).Distinct().OrderBy(store => store);
    }

    public IEnumerable<Product> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<(string StoreName, decimal MeanPrice)> GetAverageProductPricePerStore()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Product> GetSalesByStore(string storeName, int minNumberOfProducts)
    {
        throw new NotImplementedException();
    }

    public bool StoreHasProduct(string storeName, string productName)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<string> GetUniqueProducts()
    {
        throw new NotImplementedException();
    }

    public Product GetProductWithHighestMargin(string storeName)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Store> GetStoresByEstablishedYear(int year)
    {
        throw new NotImplementedException();
    }

    public decimal GetAverageProductPrice(string productName)
    {
        throw new NotImplementedException();
    }

    public decimal GetAverageProductMargin(string productName)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Product> GetFiveMostExpensiveProducts(string storeName)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<string> GetAverageProductMarginPerCountryByProductName(string productName)
    {
        throw new NotImplementedException();
    }

    public int GetNumberOfStoresByCountry(string productName, string countryName)
    {
        throw new NotImplementedException();
    }
}