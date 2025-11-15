using Pre.SalesPerStore.Core.Entities;
using Pre.SalesPerStore.Core.Interfaces;

namespace Pre.SalesPerStore.Core.Services;

public class FileService : IFileService
{
    public List<Store> LoadStoresFromFile(string fileName)
    {
        throw new NotImplementedException();
    }

    public string ReadFile(string filePath)
    {
        string fileContent;

        try
        {
            using StreamReader streamReader = new StreamReader(filePath);
            fileContent = streamReader.ReadToEnd();
        }
        catch (FileNotFoundException ex)
        {
            return $"Error: The file was not found. {ex.Message}";
        }
        catch (UnauthorizedAccessException ex)
        {
            return $"Error: You do not have permission to access this file. {ex.Message}";
        }
        catch (IOException ex)
        {
            return $"Error: The file {filePath} could not be read. {ex.Message}";
        }
        catch (Exception ex)
        {
            return $"An unexpected error occurred: {ex.Message}";
        }

        return fileContent;
    }
}