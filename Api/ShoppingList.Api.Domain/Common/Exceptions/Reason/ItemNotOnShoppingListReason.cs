using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason
{
    public class ItemNotOnShoppingListReason
    {
        public ItemNotOnShoppingListReason(ShoppingListId shoppingListId, ShoppingListItemId shoppingListItemId)
        {
            Message = $"Item {shoppingListItemId} is not on shopping list {shoppingListId.Value}.";
        }

        public string Message { get; }

        public ErrorReasonCode ErrorCode => ErrorReasonCode.ItemNotOnShoppingList;
    }
}