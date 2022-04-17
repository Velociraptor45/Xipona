using ProjectHermes.ShoppingList.Api.Domain.Common.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Reasons;

public class ItemTypeAtStoreNotAvailableReason : IReason
{
    public ItemTypeAtStoreNotAvailableReason(ItemTypeId typeId, StoreId storeId)
    {
        Message = $"Item type {typeId.Value} not available at store {storeId.Value}";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.ItemTypeAtStoreNotAvailable;
}