using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.ShoppingLists.Reasons;

public class ItemTypeAtStoreNotAvailableReason : IReason
{
    public ItemTypeAtStoreNotAvailableReason(ItemTypeId typeId, StoreId storeId)
    {
        Message = $"Item type {typeId.Value} not available at store {storeId.Value}";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.ItemTypeAtStoreNotAvailable;
}