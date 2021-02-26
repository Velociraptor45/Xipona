using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason
{
    public class ShoppingListNotFoundReason : IReason
    {
        public ShoppingListNotFoundReason(ShoppingListId id)
        {
            Message = $"Shopping list {id.Value} not found.";
        }

        public ShoppingListNotFoundReason(ShoppingListStoreId id)
        {
            Message = $"No active shopping list for store {id} found.";
        }

        public string Message { get; }

        public ErrorReasonCode ErrorCode => ErrorReasonCode.ShoppingListNotFound;
    }
}