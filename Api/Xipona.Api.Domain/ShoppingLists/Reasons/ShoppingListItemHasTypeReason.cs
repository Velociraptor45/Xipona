using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.ShoppingLists.Models;

namespace Xipona.Api.Domain.ShoppingLists.Reasons;

public class ShoppingListItemHasTypeReason : IReason
{
    public ShoppingListItemHasTypeReason(ShoppingListId shoppingListId, ItemId itemId)
    {
        Message = $"Item {itemId.Value} on shopping list {shoppingListId.Value} has types.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.ShoppingListItemHasType;
}