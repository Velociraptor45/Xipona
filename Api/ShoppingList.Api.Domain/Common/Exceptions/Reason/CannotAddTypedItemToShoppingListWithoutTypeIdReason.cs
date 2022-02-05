using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;

public class CannotAddTypedItemToShoppingListWithoutTypeIdReason : IReason
{
    public CannotAddTypedItemToShoppingListWithoutTypeIdReason(ItemId itemId)
    {
        Message = $"Cannot add typed item {itemId.Value} to shopping list without type id.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotAddTypedItemToShoppingListWithoutTypeIdReason;
}