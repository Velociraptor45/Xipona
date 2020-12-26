using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason
{
    public class ItemAtStoreNotAvailableReason : IReason
    {
        public ItemAtStoreNotAvailableReason(StoreItemId itemId, StoreId storeId)
        {
            Message = $"Item {itemId} not available at store {storeId.Value}";
        }

        public string Message { get; }

        public ErrorReasonCode ErrorCode => ErrorReasonCode.ItemAtStoreNotAvailable;
    }
}