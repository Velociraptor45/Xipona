using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services;

public interface IItemCategoryValidationService
{
    Task ValidateAsync(ItemCategoryId itemCategoryId, CancellationToken cancellationToken);
}