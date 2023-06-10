﻿using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;

public interface IRecipeModificationService
{
    Task ModifyAsync(RecipeModification modification);

    Task RemoveDefaultItemAsync(ItemId itemId);

    Task ModifyIngredientsAfterItemUpdateAsync(ItemId oldItemId, IItem newItem);

    Task ModifyIngredientsAfterAvailabilityWasDeletedAsync(ItemId itemId, ItemTypeId? itemTypeId,
        StoreId deletedAvailabilityStoreId);
}