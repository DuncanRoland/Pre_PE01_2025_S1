using Pre.SalesPerStore.Core.Entities;
using Pre.SalesPerStore.Core.Interfaces;
using Pre.SalesPerStore.Core.Services;

namespace Pre.PE01_SalesPerStore.Cons;

class Program
{
    static void Main(string[] args)
    {
        var assetsPath = Path.Combine(AppContext.BaseDirectory, "Assets");
        if (!Directory.Exists(assetsPath))
        {
            Console.Error.WriteLine($"Assets directory not found: `{assetsPath}`");
            return;
        }

        IFileService fileService = new FileService();
        var csvFiles = Directory.EnumerateFiles(assetsPath, "*.csv", SearchOption.TopDirectoryOnly).ToList();
        if (csvFiles.Count == 0)
        {
            Console.WriteLine($"No CSV files found in `{assetsPath}`");
            return;
        }

        // Test LoadStoresFromFile
        var allStores = new List<Store>();
        Console.WriteLine("Parsing CSV files:");
        foreach (var file in csvFiles)
        {
            Console.WriteLine($"\n=== {Path.GetFileName(file)} ===");
            try
            {
                var stores = fileService.LoadStoresFromFile(file);
                if (stores.Count == 0)
                {
                    Console.WriteLine("No valid store data parsed.");
                    continue;
                }

                allStores.AddRange(stores);

                foreach (var store in stores)
                {
                    Console.WriteLine($"Store: {store.StoreName} | Country: {store.StoreCountry}");
                    foreach (var product in store.Products)
                    {
                        Console.WriteLine(
                            $"  - {product.ProductName}: qty={product.Quantity}, sell={product.SellPrice}, buy={product.BuyingPrice}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to parse `{file}`: {ex.Message}");
            }
        }

        if (allStores.Count == 0)
        {
            Console.WriteLine("No stores loaded.");
            return;
        }

        // Linq
        var storeService = new StoreService(fileService, assetsPath);

        Console.WriteLine("========================================");
        Console.WriteLine("Test GetStoresByProduct linq method");
        Console.WriteLine("\nEnter product name to search (default: Laptop): ");
        var input = Console.ReadLine();
        var productName = string.IsNullOrWhiteSpace(input) ? "Laptop" : input.Trim();
        var matchingStores = storeService.GetStoresByProduct(productName).ToList();

        Console.WriteLine($"\nStores selling `{productName}`:");
        if (matchingStores.Count == 0)
            Console.WriteLine("  (none)");
        else
            foreach (var name in matchingStores)
                Console.WriteLine($"- {name}");

        // Test GetAllCountries linq method
        Console.WriteLine("========================================");
        Console.WriteLine("test GetAllCountries linq method");
        var countries = storeService.GetAllCountries().ToList();
        Console.WriteLine("\nCountries with stores:");
        foreach (var country in countries)
        {
            Console.WriteLine($"- {country}");
        }

        // Test GetAllStores linq method
        Console.WriteLine("========================================");
        Console.WriteLine("test GetAllStores linq method");
        var storesList = storeService.GetAllStores().ToList();
        Console.WriteLine("\nAll unique stores:");
        foreach (var store in storesList)
        {
            Console.WriteLine($"- {store}");
        }

        // Test GetProductsByPriceRange linq method
        Console.WriteLine("========================================");
        Console.WriteLine("test GetProductsByPriceRange linq method");
        Console.WriteLine("\nEnter minimum price (default: 100): ");
        var inputMin = Console.ReadLine();

        Console.WriteLine("Enter maximum price (default: 500): ");
        var inputMax = Console.ReadLine();

        decimal minPrice = string.IsNullOrWhiteSpace(inputMin) ? 100 : decimal.Parse(inputMin);
        decimal maxPrice = string.IsNullOrWhiteSpace(inputMax) ? 500 : decimal.Parse(inputMax);
        var productsInRange = storeService.GetProductsByPriceRange(minPrice, maxPrice).ToList();
        Console.WriteLine($"\nProducts with price between {minPrice} and {maxPrice}:");
        foreach (var product in productsInRange)
        {
            Console.WriteLine($"- {product.ProductName}: sell={product.SellPrice}, buy={product.BuyingPrice}");
        }

        // Test GetAverageProductPricePerStore linq method
        Console.WriteLine("========================================");
        Console.WriteLine("test GetAverageProductPricePerStore linq method");
        var averagePrices = storeService.GetAverageProductPricePerStore().ToList();
        Console.WriteLine("\nAverage product prices per store:");
        foreach (var (storeName, meanPrice) in averagePrices)
        {
            Console.WriteLine($"- {storeName}: Average Price = {meanPrice}");
        }

        // Test GetSalesByStore linq method
        Console.WriteLine("========================================");
        Console.WriteLine("test GetSalesByStore linq method");
        Console.WriteLine("\nEnter store name to search (default: TechWorld): ");
        var storeInput = Console.ReadLine();
        var storeNameToSearch = string.IsNullOrWhiteSpace(storeInput) ? "TechWorld" : storeInput.Trim();
        Console.WriteLine("Enter minimum number of products (default: 10): ");
        var minProductsInput = Console.ReadLine();
        int minNumberOfProducts = string.IsNullOrWhiteSpace(minProductsInput) ? 10 : int.Parse(minProductsInput);
        var lowStockProducts = storeService.GetSalesByStore(storeNameToSearch, minNumberOfProducts).ToList();
        Console.WriteLine($"\nProducts in `{storeNameToSearch}` with stock less than {minNumberOfProducts}:");
        if (lowStockProducts.Count == 0)
            Console.WriteLine("  (none)");
        else
            foreach (var product in lowStockProducts)
                Console.WriteLine(
                    $"- {product.ProductName}: qty={product.Quantity}, sell={product.SellPrice}, buy={product.BuyingPrice}");

        // Test StoreHasProduct linq method
        Console.WriteLine("========================================");
        Console.WriteLine("test StoreHasProduct linq method");
        Console.WriteLine("\nEnter store name to search (default: TechWorld): ");
        var storeInput2 = Console.ReadLine();
        var storeNameToCheck = string.IsNullOrWhiteSpace(storeInput2) ? "TechWorld" : storeInput2.Trim();
        Console.WriteLine("Enter product name to search (default: Laptop): ");
        var productInput2 = Console.ReadLine();
        var productNameToCheck = string.IsNullOrWhiteSpace(productInput2) ? "Laptop" : productInput2.Trim();
        var hasProduct = storeService.StoreHasProduct(storeNameToCheck, productNameToCheck);
        Console.WriteLine(hasProduct
            ? $"\nYes, `{storeNameToCheck}` sells `{productNameToCheck}`."
            : $"\n{storeNameToCheck}` does not sell `{productNameToCheck}`.");

        // Test GetUniqueProducts linq method
        Console.WriteLine("========================================");
        Console.WriteLine("test GetUniqueProducts linq method");
        var uniqueProducts = storeService.GetUniqueProducts().ToList();
        Console.WriteLine("\nUnique products (from expensive to cheap):");
        foreach (var product in uniqueProducts)
        {
            Console.WriteLine($"- {product}");
        }

        // Test GetProductWithHighestMargin linq method
        Console.WriteLine("========================================");
        Console.WriteLine("test GetProductWithHighestMargin linq method");
        Console.WriteLine("\nEnter store name to search (default: TechWorld): ");
        var storeInput3 = Console.ReadLine();
        var storeNameForMargin = string.IsNullOrWhiteSpace(storeInput3) ? "TechWorld" : storeInput3.Trim();
        var highestMarginProduct = storeService.GetProductWithHighestMargin(storeNameForMargin);
        if (!highestMarginProduct.Equals(null))
        {
            var margin = highestMarginProduct.SellPrice - highestMarginProduct.BuyingPrice;
            Console.WriteLine($"\nProduct with highest margin in `{storeNameForMargin}`:");
            Console.WriteLine(
                $"- {highestMarginProduct.ProductName}: sell={highestMarginProduct.SellPrice}, buy={highestMarginProduct.BuyingPrice}, margin={margin}");
        }
        else
        {
            Console.WriteLine($"\nNo products found in `{storeNameForMargin}`.");
        }
    }
}