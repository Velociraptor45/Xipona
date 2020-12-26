using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason
{
    public class ItemNotTemporaryReason : IReason
    {
        public ItemNotTemporaryReason(StoreItemId id)
        {
            Message = $"Item {id} is not temporary.";
        }

        public string Message { get; }

        public ErrorReasonCode ErrorCode => ErrorReasonCode.ItemNotTemporary;
    }
}