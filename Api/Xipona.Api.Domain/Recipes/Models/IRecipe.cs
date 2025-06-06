using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Recipes.Services.Modifications;
using Xipona.Api.Domain.RecipeTags.Models;
using Xipona.Api.Domain.Shared.Validations;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Domain.Recipes.Models;

public interface IRecipe
{
    RecipeId Id { get; }
    RecipeName Name { get; }
    NumberOfServings NumberOfServings { get; }
    IReadOnlyCollection<IIngredient> Ingredients { get; }
    IReadOnlyCollection<IPreparationStep> PreparationSteps { get; }
    IReadOnlyCollection<RecipeTagId> Tags { get; }
    RecipeId? SideDishId { get; }
    DateTimeOffset CreatedAt { get; }

    Task ModifyAsync(RecipeModification modification, IValidator validator);

    void RemoveDefaultItem(ItemId defaultItemId, ItemTypeId? itemTypeId);

    void ModifyIngredientsAfterItemUpdate(ItemId oldItemId, IItem newItem);

    void ModifyIngredientsAfterAvailabilityWasDeleted(ItemId itemId, ItemTypeId? itemTypeId, IItem item,
        StoreId deletedAvailabilityStoreId);

    void ModifyIngredientsAfterAvailabilitiesChanged(ItemId itemId, ItemTypeId? itemTypeId,
        IEnumerable<ItemAvailability> newAvailabilities);

    void RemoveIngredientsOfItemCategory(ItemCategoryId itemCategoryId);
}