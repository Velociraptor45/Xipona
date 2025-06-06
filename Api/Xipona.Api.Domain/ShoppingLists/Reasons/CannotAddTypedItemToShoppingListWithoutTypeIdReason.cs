using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.Items.Models;

namespace Xipona.Api.Domain.ShoppingLists.Reasons;

public class CannotAddTypedItemToShoppingListWithoutTypeIdReason : IReason
{
    public CannotAddTypedItemToShoppingListWithoutTypeIdReason(ItemId itemId)
    {
        Message = $"Cannot add typed item {itemId.Value} to shopping list without type id.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotAddTypedItemToShoppingListWithoutTypeId;
}