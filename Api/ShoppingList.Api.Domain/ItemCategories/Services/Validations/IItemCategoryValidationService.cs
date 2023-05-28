using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Validations;

public interface IItemCategoryValidationService
{
    Task ValidateAsync(ItemCategoryId itemCategoryId);
}