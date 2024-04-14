using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.ShoppingLists.Reasons;

public class ItemAtStoreNotAvailableReason : IReason
{
    public ItemAtStoreNotAvailableReason(ItemId itemId, StoreId storeId)
    {
        Message = $"Item {itemId.Value} not available at store {storeId.Value}";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.ItemAtStoreNotAvailable;
}