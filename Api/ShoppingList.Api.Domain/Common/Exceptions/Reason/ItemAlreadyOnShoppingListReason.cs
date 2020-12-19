using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason
{
    public class ItemAlreadyOnShoppingListReason : IReason
    {
        public ItemAlreadyOnShoppingListReason(ShoppingListItemId itemId, ShoppingListId listId)
        {
            Message = $"Item {itemId} already exists on shopping list {listId.Value}";
        }

        public string Message { get; }

        public ErrorReasonCode ErrorCode => ErrorReasonCode.ItemAlreadyOnShoppingList;
    }
}