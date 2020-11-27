using ShoppingList.Api.Domain.Models;

namespace ShoppingList.Api.Domain.Extensions
{
    public static class ShoppingListItemIdExtensions
    {
        public static StoreItemId ToStoreItemId(this ShoppingListItemId shoppingListItemId)
        {
            return shoppingListItemId.IsActualId ?
                new StoreItemId(shoppingListItemId.Actual.Value) :
                new StoreItemId(shoppingListItemId.Offline.Value);
        }
    }
}