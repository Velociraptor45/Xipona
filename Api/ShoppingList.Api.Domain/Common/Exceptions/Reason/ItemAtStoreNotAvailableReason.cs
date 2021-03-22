using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Model;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason
{
    public class ItemAtStoreNotAvailableReason : IReason
    {
        public ItemAtStoreNotAvailableReason(ItemId itemId, StoreId storeId)
        {
            Message = $"Item {itemId} not available at store {storeId.Value}";
        }

        public string Message { get; }

        public ErrorReasonCode ErrorCode => ErrorReasonCode.ItemAtStoreNotAvailable;
    }
}