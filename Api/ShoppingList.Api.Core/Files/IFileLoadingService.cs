namespace ProjectHermes.ShoppingList.Api.Core.Files;

public interface IFileLoadingService
{
    string ReadFile(string filePath);
}