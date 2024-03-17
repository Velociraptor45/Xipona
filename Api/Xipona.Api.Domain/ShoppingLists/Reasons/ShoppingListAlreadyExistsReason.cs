using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.ShoppingLists.Reasons;

public class ShoppingListAlreadyExistsReason : IReason
{
    public ShoppingListAlreadyExistsReason(StoreId storeId)
    {
        Message = $"There's already an active shoppingList for store {storeId.Value}.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.ShoppingListAlreadyExists;
}