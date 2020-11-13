using ShoppingList.Api.Domain.Models;

namespace ShoppingList.Api.Domain.Extensions
{
    public static class StoreItemIdExtensions
    {
        public static ShoppingListItemId ToShoppingListItemId(this StoreItemId storeItemId)
        {
            return new ShoppingListItemId(storeItemId.Value);
        }
    }
}