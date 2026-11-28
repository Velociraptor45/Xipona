using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Domain.Recipes.Services.Modifications;

public interface IRecipeModificationService
{
    Task ModifyAsync(RecipeModification modification);

    Task RemoveDefaultItemAsync(ItemId itemId, ItemTypeId? itemTypeId);

    Task ModifyIngredientsAfterItemUpdateAsync(ItemId oldItemId, IItem newItem);

    Task ModifyIngredientsAfterAvailabilityWasDeletedAsync(ItemId itemId, ItemTypeId? itemTypeId,
        StoreId deletedAvailabilityStoreId);

    Task ModifyIngredientsAfterAvailabilitiesChangedAsync(ItemId itemId, ItemTypeId? itemTypeId,
        IEnumerable<ItemAvailability> newAvailabilities);

    Task RemoveIngredientsOfItemCategoryAsync(ItemCategoryId itemCategoryId);
    Task ReplaceMergedItemAsync(ItemId originalItemId, ItemId newItemId, ItemTypeId newItemTypeId);
}