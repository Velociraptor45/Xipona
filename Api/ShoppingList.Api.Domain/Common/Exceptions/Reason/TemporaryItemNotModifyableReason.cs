using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason
{
    public class TemporaryItemNotModifyableReason : IReason
    {
        public TemporaryItemNotModifyableReason(ItemId id)
        {
            Message = $"Item {id} is temporary and thus cannot be modified.";
        }

        public string Message { get; }

        public ErrorReasonCode ErrorCode => ErrorReasonCode.TemporaryItemNotModifyable;
    }
}