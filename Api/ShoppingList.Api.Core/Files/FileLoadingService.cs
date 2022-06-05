using System.IO;

namespace ProjectHermes.ShoppingList.Api.Core.Files;

public class FileLoadingService : IFileLoadingService
{
    public string ReadFile(string filePath)
    {
        if (!File.Exists(filePath))
            throw new InvalidOperationException($"File {filePath} does not exist");

        return File.ReadAllText(filePath);
    }
}