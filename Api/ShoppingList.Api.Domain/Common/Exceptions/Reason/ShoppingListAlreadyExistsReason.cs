using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason
{
    public class ShoppingListAlreadyExistsReason : IReason
    {
        public ShoppingListAlreadyExistsReason(ShoppingListStoreId storeId)
        {
            Message = $"There's already an active shoppingList for store {storeId.Value}.";
        }

        public string Message { get; }

        public ErrorReasonCode ErrorCode => ErrorReasonCode.ShoppingListAlreadyExists;
    }
}