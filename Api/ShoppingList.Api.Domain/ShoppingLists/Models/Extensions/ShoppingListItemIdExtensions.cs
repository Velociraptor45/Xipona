using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Extensions
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