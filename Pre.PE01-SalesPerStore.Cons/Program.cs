using Pre.SalesPerStore.Core.Interfaces;
using Pre.SalesPerStore.Core.Services;

namespace Pre.PE01_SalesPerStore.Cons;

class Program
{
    static void Main(string[] args)
    {
        string folderPath = Path.Combine("G:", "My Drive", "howest", "Programming_Expert", "PE1",
            "st-pe-store-start-DuncanRoland",
            "Pre.PE01-SalesPerStore.Cons", "Assets");

        if (!Directory.Exists(folderPath))
        {
            Console.Error.WriteLine($"Directory not found: `{folderPath}`");
            return;
        }

        IFileService fileService = new FileService();
        //string filePath = Path.Combine(folderPath, "stores_products.csv");

        var csvFiles = Directory.EnumerateFiles(folderPath, "*.csv", SearchOption.TopDirectoryOnly);

        bool foundAny = false;

        foreach (var file in csvFiles)
        {
            foundAny = true;
            Console.WriteLine($"\n=== {Path.GetFileName(file)} ===");
            try
            {
                var stores = fileService.LoadStoresFromFile(file);
                if (stores.Count == 0)
                {
                    Console.WriteLine("No valid store data parsed.");
                    continue;
                }

                foreach (var store in stores)
                {
                    Console.WriteLine($"Store: {store.StoreName} | Country: {store.StoreCountry}");
                    foreach (var product in store.Products)
                    {
                        Console.WriteLine($"  - {product.ProductName}: qty={product.Quantity}, sell={product.SellPrice}, buy={product.BuyingPrice}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to parse `{file}`: {ex.Message}");
            }
        }

        if (!foundAny)
        {
            Console.WriteLine($"No CSV files found in `{folderPath}`");
        }
    }
}