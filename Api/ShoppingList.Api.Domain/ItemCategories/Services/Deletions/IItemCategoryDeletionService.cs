using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Deletions;

public interface IItemCategoryDeletionService
{
    Task DeleteAsync(ItemCategoryId itemCategoryId);
}