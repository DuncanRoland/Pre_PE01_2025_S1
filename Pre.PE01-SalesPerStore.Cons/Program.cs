using Pre.SalesPerStore.Core.Interfaces;
using Pre.SalesPerStore.Core.Services;

namespace Pre.PE01_SalesPerStore.Cons;

class Program
{
    static void Main(string[] args)
    {
        var folderPath = Path.Combine("G:", "My Drive", "howest", "Programming_Expert", "PE1",
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

        foreach (var file in csvFiles)
        {
            Console.WriteLine($"\n--- {Path.GetFileName(file)} ---");
            try
            {
                var content = fileService.ReadFile(file);
                Console.WriteLine(content);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read `{file}`: {ex.Message}");
            }
        }
    }
}