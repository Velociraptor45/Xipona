namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Extensions
{
    public static class ShoppingListItemIdExtensions
    {
        public static ItemId ToStoreItemId(this ItemId shoppingListItemId)
        {
            return shoppingListItemId.IsActualId ?
                new StoreItemId(shoppingListItemId.Actual.Value) :
                new StoreItemId(shoppingListItemId.Offline.Value);
        }
    }
}