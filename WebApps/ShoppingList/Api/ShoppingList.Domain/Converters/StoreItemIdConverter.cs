using ShoppingList.Domain.Models;

namespace ShoppingList.Domain.Converters
{
    public static class StoreItemIdConverter
    {
        public static ShoppingListItemId ToShoppingListItemId(this StoreItemId storeItemId)
        {
            return new ShoppingListItemId(storeItemId.Value);
        }
    }
}