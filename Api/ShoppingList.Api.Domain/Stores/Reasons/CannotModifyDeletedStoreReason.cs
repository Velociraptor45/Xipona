using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Reasons;

public class CannotModifyDeletedStoreReason : IReason
{
    public CannotModifyDeletedStoreReason(StoreId storeId)
    {
        Message = $"Cannot modify deleted store ({storeId.Value})";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotModifyDeletedStore;
}