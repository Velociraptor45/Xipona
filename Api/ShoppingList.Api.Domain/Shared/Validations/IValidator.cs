using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;

public interface IValidator
{
    Task ValidateAsync(IEnumerable<IItemAvailability> availabilities);

    Task ValidateAsync(ItemCategoryId itemCategoryId);

    Task ValidateAsync(ManufacturerId manufacturerId);

    Task ValidateAsync(ItemId itemId, ItemTypeId? itemTypeId);

    Task ValidateAsync(IEnumerable<RecipeTagId> recipeTagIds);
}