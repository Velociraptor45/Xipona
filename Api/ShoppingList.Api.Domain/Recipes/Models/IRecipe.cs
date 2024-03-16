using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

public interface IRecipe
{
    RecipeId Id { get; }
    RecipeName Name { get; }
    NumberOfServings NumberOfServings { get; }
    IReadOnlyCollection<IIngredient> Ingredients { get; }
    IReadOnlyCollection<IPreparationStep> PreparationSteps { get; }
    IReadOnlyCollection<RecipeTagId> Tags { get; }
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