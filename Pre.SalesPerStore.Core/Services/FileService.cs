using System.Globalization;
using System.Text;
using Pre.SalesPerStore.Core.Entities;
using Pre.SalesPerStore.Core.Events;
using Pre.SalesPerStore.Core.Interfaces;

namespace Pre.SalesPerStore.Core.Services;

public class FileService : IFileService
{
    public event EventHandler<PrintEventArgs>? PrintEventArgsOccurred;

    protected virtual void OnPrintEventArgsOccurred(PrintEventArgs e)
        => PrintEventArgsOccurred?.Invoke(this, e);


    public List<Store> LoadStoresFromFile(string fileName)
    {
        var storesByKey = new Dictionary<string, Store?>(StringComparer.OrdinalIgnoreCase);

        var lines = ReadFile(fileName);

        for (int i = 0; i < lines.Length; i++)
        {
            int lineNumber = i + 1;
            var trimmed = lines[i].Trim();

            if (lineNumber == 1 && trimmed.StartsWith("StoreName", StringComparison.OrdinalIgnoreCase))
                continue;

            var parts = trimmed.Split(';');
            if (parts.Length < 7)
            {
                Console.Error.WriteLine($"[LoadStoresFromFile] Malformed line {lineNumber}.");
                continue;
            }

            try
            {
                var storeName = parts[0].Trim();
                var storeCountry = parts[1].Trim();
                DateTime establishedDate =
                    DateTime.ParseExact(parts[2].Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                var productName = parts[3].Trim();
                int quantity = int.Parse(parts[4].Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture);
                decimal sellPrice = decimal.Parse(parts[5].Trim(), NumberStyles.Number, CultureInfo.InvariantCulture);
                decimal buyingPrice = decimal.Parse(parts[6].Trim(), NumberStyles.Number, CultureInfo.InvariantCulture);

                var key = storeName + "|" + storeCountry + "|" + establishedDate.ToString("yyyy-MM-dd");
                if (!storesByKey.TryGetValue(key, out var store))
                {
                    store = new Store(storeName, storeCountry, establishedDate);
                    storesByKey[key] = store;
                }

                store?.Products.Add(new Product(productName, quantity, sellPrice, buyingPrice));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[LoadStoresFromFile] Skipping line {lineNumber}: {ex.Message}");
            }
        }

        return new List<Store>(storesByKey.Values!);
    }


    public string[] ReadFile(string filePath, Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;

        try
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                OnPrintEventArgsOccurred(new PrintEventArgs("CreateDirectory", "SUCCESS"));
            }

            if (!File.Exists(filePath))
            {
                OnPrintEventArgsOccurred(new PrintEventArgs("FileMissing", "FAILED", "File doesn't exist"));
                return Array.Empty<string>();
            }

            using var sr = new StreamReader(filePath, encoding);
            var content = sr.ReadToEnd();
            OnPrintEventArgsOccurred(new PrintEventArgs("ReadFileSuccess", "SUCCESS"));
            return content.Split(["\r\n", "\n"], StringSplitOptions.None);
        }
        catch (Exception ex)
        {
            OnPrintEventArgsOccurred(new PrintEventArgs("ReadFileFailure", "FAILED", ex.Message));
            return Array.Empty<string>();
        }
    }
}