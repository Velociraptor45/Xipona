using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Manufacturers.Models;
using Xipona.Api.Domain.RecipeTags.Models;

namespace Xipona.Api.Domain.Shared.Validations;

public interface IValidator
{
    Task ValidateAsync(IEnumerable<ItemAvailability> availabilities);

    Task ValidateAsync(ItemCategoryId itemCategoryId);

    Task ValidateAsync(ManufacturerId manufacturerId);

    Task ValidateAsync(ItemId itemId, ItemTypeId? itemTypeId);

    Task ValidateAsync(IEnumerable<RecipeTagId> recipeTagIds);
}