using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Modifications;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;
using ProjectHermes.Xipona.Api.Domain.Shared.Validations;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.Models;

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