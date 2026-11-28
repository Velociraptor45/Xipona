using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Domain.ShoppingLists.Reasons;

public class ShoppingListAlreadyExistsReason : IReason
{
    public ShoppingListAlreadyExistsReason(StoreId storeId)
    {
        Message = $"There's already an active shoppingList for store {storeId.Value}.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.ShoppingListAlreadyExists;
}