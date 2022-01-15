using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason
{
    public class ShoppingListItemHasTypeReason : IReason
    {
        public ShoppingListItemHasTypeReason(ShoppingListId shoppingListId, ItemId itemId)
        {
            Message = $"Item {itemId.Value} on shopping list {shoppingListId.Value} has types.";
        }

        public string Message { get; }

        public ErrorReasonCode ErrorCode => ErrorReasonCode.ShoppingListItemHasType;
    }
}