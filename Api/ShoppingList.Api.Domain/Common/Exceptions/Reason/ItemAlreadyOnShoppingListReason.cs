using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason
{
    public class ItemAlreadyOnShoppingListReason : IReason
    {
        public ItemAlreadyOnShoppingListReason(StoreItemId itemId, ShoppingListId listId)
        {
            Message = $"Item {itemId} already exists on shopping list {listId.Value}";
        }

        public string Message { get; }

        public ErrorReasonCode ErrorCode => ErrorReasonCode.ItemAlreadyOnShoppingList;
    }
}