using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;

public class ItemTypeNotFoundReason : IReason
{
    public ItemTypeNotFoundReason(ItemId itemId, ItemTypeId itemTypeId)
    {
        Message = $"Item {itemId.Value} does not contain item type {itemTypeId.Value}.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.ItemTypeNotFound;
}