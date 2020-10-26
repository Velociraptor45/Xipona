using ShoppingList.Api.Domain.Models;

namespace ShoppingList.Api.Domain.Converters
{
    public static class StoreItemIdConverter
    {
        public static ShoppingListItemId ToShoppingListItemId(this StoreItemId storeItemId)
        {
            return new ShoppingListItemId(storeItemId.Value);
        }
    }
}