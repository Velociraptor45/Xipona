using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason
{
    public class CannotCreateItemWithTypesWithoutTypesReason : IReason
    {
        public CannotCreateItemWithTypesWithoutTypesReason(ItemId itemId)
        {
            Message = $"Cannot create item with types without types (id: {itemId.Value}.";
        }

        public string Message { get; }

        public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotCreateItemWithTypesWithoutTypes;
    }
}