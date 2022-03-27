using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Creations;

public interface IItemCategoryCreationService
{
    Task<IItemCategory> CreateAsync(ItemCategoryName name);
}