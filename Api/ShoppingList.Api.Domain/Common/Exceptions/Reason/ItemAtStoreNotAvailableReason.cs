using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason
{
    public class ItemAtStoreNotAvailableReason : IReason
    {
        public ItemAtStoreNotAvailableReason(StoreItemId itemId, StoreItemStoreId storeId)
        {
            Message = $"Item {itemId} not available at store {storeId.Value}";
        }

        public ItemAtStoreNotAvailableReason(StoreItemId itemId, ShoppingListStoreId storeId)
        {
            Message = $"Item {itemId} not available at store {storeId.Value}";
        }

        public string Message { get; }

        public ErrorReasonCode ErrorCode => ErrorReasonCode.ItemAtStoreNotAvailable;
    }
}