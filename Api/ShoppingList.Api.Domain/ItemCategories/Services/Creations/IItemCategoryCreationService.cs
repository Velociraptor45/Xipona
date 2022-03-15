namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Creations;

public interface IItemCategoryCreationService
{
    Task CreateAsync(string name);
}