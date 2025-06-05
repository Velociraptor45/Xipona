using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Domain.ShoppingLists.Reasons;

public class ItemAtStoreNotAvailableReason : IReason
{
    public ItemAtStoreNotAvailableReason(ItemId itemId, StoreId storeId)
    {
        Message = $"Item {itemId.Value} not available at store {storeId.Value}";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.ItemAtStoreNotAvailable;
}