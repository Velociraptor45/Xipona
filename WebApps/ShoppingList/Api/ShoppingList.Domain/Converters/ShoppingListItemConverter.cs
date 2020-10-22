using ShoppingList.Domain.Models;

namespace ShoppingList.Domain.Converters
{
    public static class ShoppingListItemConverter
    {
        public static StoreItemId ToStoreItemId(this ShoppingListItemId shoppingListItemId)
        {
            return new StoreItemId(shoppingListItemId.Value);
        }
    }
}