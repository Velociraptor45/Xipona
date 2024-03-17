using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;

namespace ProjectHermes.Xipona.Api.Domain.Shared.Validations;

public interface IValidator
{
    Task ValidateAsync(IEnumerable<ItemAvailability> availabilities);

    Task ValidateAsync(ItemCategoryId itemCategoryId);

    Task ValidateAsync(ManufacturerId manufacturerId);

    Task ValidateAsync(ItemId itemId, ItemTypeId? itemTypeId);

    Task ValidateAsync(IEnumerable<RecipeTagId> recipeTagIds);
}