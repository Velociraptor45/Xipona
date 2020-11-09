using ShoppingList.Api.Domain.Models;

namespace ShoppingList.Api.Domain.Extensions
{
    public static class ShoppingListItemIdExtensions
    {
        public static StoreItemId ToStoreItemId(this ShoppingListItemId shoppingListItemId)
        {
            return new StoreItemId(shoppingListItemId.Value);
        }
    }
}