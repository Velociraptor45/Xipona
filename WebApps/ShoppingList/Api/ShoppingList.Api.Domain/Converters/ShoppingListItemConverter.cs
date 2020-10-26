using ShoppingList.Api.Domain.Models;

namespace ShoppingList.Api.Domain.Converters
{
    public static class ShoppingListItemConverter
    {
        public static StoreItemId ToStoreItemId(this ShoppingListItemId shoppingListItemId)
        {
            return new StoreItemId(shoppingListItemId.Value);
        }
    }
}