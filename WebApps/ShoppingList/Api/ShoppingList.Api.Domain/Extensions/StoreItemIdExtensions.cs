using ShoppingList.Api.Domain.Models;

namespace ShoppingList.Api.Domain.Extensions
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