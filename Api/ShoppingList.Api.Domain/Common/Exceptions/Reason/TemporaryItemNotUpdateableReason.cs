using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason
{
    public class TemporaryItemNotUpdateableReason : IReason
    {
        public TemporaryItemNotUpdateableReason(ItemId id)
        {
            Message = $"Item {id} is temporary and thus cannot be updated.";
        }

        public string Message { get; }

        public ErrorReasonCode ErrorCode => ErrorReasonCode.TemporaryItemNotUpdateable;
    }
}