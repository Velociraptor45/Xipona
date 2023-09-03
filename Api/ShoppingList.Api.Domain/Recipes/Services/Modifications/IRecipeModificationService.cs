using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;

public interface IRecipeModificationService
{
    Task ModifyAsync(RecipeModification modification);

    Task RemoveDefaultItemAsync(ItemId itemId, ItemTypeId? itemTypeId);

    Task ModifyIngredientsAfterItemUpdateAsync(ItemId oldItemId, IItem newItem);

    Task ModifyIngredientsAfterAvailabilityWasDeletedAsync(ItemId itemId, ItemTypeId? itemTypeId,
        StoreId deletedAvailabilityStoreId);

    Task ModifyIngredientsAfterAvailabilitiesChangedAsync(ItemId itemId, ItemTypeId? itemTypeId,
        IEnumerable<ItemAvailability> oldAvailabilities, IEnumerable<ItemAvailability> newAvailabilities);

    Task RemoveIngredientsOfItemCategoryAsync(ItemCategoryId itemCategoryId);
}