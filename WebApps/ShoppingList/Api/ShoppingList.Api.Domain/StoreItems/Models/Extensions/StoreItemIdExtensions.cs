﻿using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Extensions
{
    public static class StoreItemIdExtensions
    {
        public static ShoppingListItemId ToShoppingListItemId(this StoreItemId storeItemId)
        {
            return storeItemId.IsActualId ?
                new ShoppingListItemId(storeItemId.Actual.Value) :
                new ShoppingListItemId(storeItemId.Offline.Value);
        }
    }
}