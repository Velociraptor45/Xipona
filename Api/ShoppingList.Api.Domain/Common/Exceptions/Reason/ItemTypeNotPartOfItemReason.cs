using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason
{
    public class ItemTypeNotPartOfItemReason : IReason
    {
        public ItemTypeNotPartOfItemReason(ItemId itemId, ItemTypeId itemTypeId)
        {
            Message = $"Item type {itemTypeId.Value} is not part of item {itemId.Value}";
        }

        public string Message { get; }

        public ErrorReasonCode ErrorCode => ErrorReasonCode.ItemTypeNotPartOfItem;
    }
}