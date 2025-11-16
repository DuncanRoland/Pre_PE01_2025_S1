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
    }
}