using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Creations;

public interface IItemCategoryCreationService
{
    Task CreateAsync(ItemCategoryName name);
}