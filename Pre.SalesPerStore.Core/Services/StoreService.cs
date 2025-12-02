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
        return _stores.SelectMany(store => store.Products)
            .Where(product => product.SellPrice >= minPrice && product.SellPrice <= maxPrice);
    }

    public IEnumerable<(string StoreName, decimal MeanPrice)> GetAverageProductPricePerStore()
    {
        return _stores
            .GroupBy(store => store.StoreName)
            .Select(grouping =>
            {
                var allSellPrices = grouping
                    .SelectMany(store => store.Products)
                    .Select(product => product.SellPrice);

                var mean = allSellPrices.DefaultIfEmpty(0m).Average();
                return (StoreName: grouping.Key, MeanPrice: mean);
            });
    }

    public IEnumerable<Product> GetSalesByStore(string storeName, int minNumberOfProducts)
    {
        return _stores
            .Where(store => string.Equals(store.StoreName, storeName, StringComparison.OrdinalIgnoreCase))
            .SelectMany(store => store.Products)
            .Where(product => product.Quantity < minNumberOfProducts)
            .OrderBy(product => product.Quantity);
    }

    public bool StoreHasProduct(string storeName, string productName)
    {
        return _stores.Any(store =>
            string.Equals(store.StoreName, storeName, StringComparison.OrdinalIgnoreCase) &&
            store.Products.Any(product =>
                string.Equals(product.ProductName, productName, StringComparison.OrdinalIgnoreCase)));
    }

    public IEnumerable<string> GetUniqueProducts()
    {
        return _stores.SelectMany(store => store.Products)
            .Select(product => product.ProductName)
            .Distinct()
            .OrderByDescending(productName => productName);
    }

    public Product GetProductWithHighestMargin(string storeName)
    {
        return _stores.Where(store => string.Equals(store.StoreName, storeName, StringComparison.OrdinalIgnoreCase))
                   .SelectMany(store => store.Products)
                   .MaxBy(product => product.SellPrice - product.BuyingPrice) ??
               throw new InvalidOperationException("No products found for store '{storeName}'");
    }

    public IEnumerable<Store> GetStoresByEstablishedYear(int year)
    {
        return _stores.Where(store => store.EstablishedDate.Year < year).OrderBy(store => store.EstablishedDate);
    }

    public decimal GetAverageProductPrice(string productName)
    {
        return _stores.SelectMany(store => store.Products)
            .Where(product => string.Equals(product.ProductName, productName, StringComparison.OrdinalIgnoreCase))
            .Select(product => product.SellPrice)
            .DefaultIfEmpty(0m)
            .Average();
    }

    public decimal GetAverageProductMargin(string productName)
    {
        return _stores.SelectMany(store => store.Products)
            .Where(product => string.Equals(product.ProductName, productName, StringComparison.OrdinalIgnoreCase))
            .Select(product => product.SellPrice - product.BuyingPrice)
            .DefaultIfEmpty(0m)
            .Average();
    }

    public IEnumerable<Product> GetFiveMostExpensiveProducts(string storeName)
    {
        return _stores.Where(store => string.Equals(store.StoreName, storeName, StringComparison.OrdinalIgnoreCase))
            .SelectMany(store => store.Products)
            .OrderByDescending(product => product.SellPrice)
            .Take(5);
    }

    public IEnumerable<string> GetAverageProductMarginPerCountryByProductName(string productName)
    {
        /*return _stores.SelectMany(store => store.Products
                .Where(product => string.Equals(product.ProductName, productName, StringComparison.OrdinalIgnoreCase))
                .Select(product => new
                {
                    store.StoreCountry,
                    Margin = product.SellPrice - product.BuyingPrice
                }))
            .GroupBy(item => item.StoreCountry)
            .Select(grouping =>
            {
                var averageMargin = grouping
                    .Select(item => item.Margin)
                    .DefaultIfEmpty(0m)
                    .Average();

                return new
                {
                    Country = grouping.Key,
                    AverageMargin = averageMargin
                };
            })
            .OrderBy(item => item.AverageMargin)
            .Select(item => $"Country: {item.Country} - Margin: {item.AverageMargin:F2}");*/

        //Eenvoudiger
        var productMarginsByCountry = _stores
            .SelectMany(store => store.Products, (store, product) => new
            {
                store.StoreCountry,
                Product = product
            })
            .Where(x => string.Equals(
                x.Product.ProductName,
                productName,
                StringComparison.OrdinalIgnoreCase))
            .Select(x => new
            {
                x.StoreCountry,
                Margin = x.Product.SellPrice - x.Product.BuyingPrice
            });

        var averageMarginsByCountry = productMarginsByCountry
            .GroupBy(x => x.StoreCountry)
            .Select(group => new
            {
                Country = group.Key,
                AverageMargin = group
                    .Select(x => x.Margin)
                    .DefaultIfEmpty(0m)
                    .Average()
            })
            .OrderBy(x => x.AverageMargin);

        return averageMarginsByCountry
            .Select(x => $"Country: {x.Country} - Margin: {x.AverageMargin:F2}");
    }

    public int GetNumberOfStoresByCountry(string productName, string countryName)
    {
        return _stores
            .Count(store => string.Equals(store.StoreCountry, countryName, StringComparison.OrdinalIgnoreCase) &&
                            store.Products.Any(product =>
                                string.Equals(product.ProductName, productName, StringComparison.OrdinalIgnoreCase)));
    }
}