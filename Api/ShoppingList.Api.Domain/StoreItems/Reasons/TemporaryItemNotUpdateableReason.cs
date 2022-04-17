using ProjectHermes.ShoppingList.Api.Domain.Common.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Reasons;

public class TemporaryItemNotUpdateableReason : IReason
{
    public TemporaryItemNotUpdateableReason(ItemId id)
    {
        Message = $"Item {id} is temporary and thus cannot be updated.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.TemporaryItemNotUpdateable;
}