using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason
{
    public class ItemNotFoundReason : IReason
    {
        public ItemNotFoundReason(ItemId id)
        {
            Message = $"Item {id} not found.";
            ErrorCode = ErrorReasonCode.ItemNotFound;
        }

        public string Message { get; }

        public ErrorReasonCode ErrorCode { get; }
    }
}