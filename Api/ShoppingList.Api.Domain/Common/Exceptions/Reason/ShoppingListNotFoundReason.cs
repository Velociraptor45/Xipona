using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason
{
    public class ShoppingListNotFoundReason : IReason
    {
        public ShoppingListNotFoundReason(ShoppingListId id)
        {
            Message = $"Shopping list {id.Value} not found.";
        }

        public string Message { get; }

        public ErrorReasonCode ErrorCode => ErrorReasonCode.ShoppingListNotFound;
    }
}