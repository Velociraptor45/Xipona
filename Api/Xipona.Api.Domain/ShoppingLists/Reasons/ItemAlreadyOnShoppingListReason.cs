using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.ShoppingLists.Models;

namespace Xipona.Api.Domain.ShoppingLists.Reasons;

public class ItemAlreadyOnShoppingListReason : IReason
{
    public ItemAlreadyOnShoppingListReason(ItemId itemId, ShoppingListId listId)
    {
        Message = $"Item {itemId.Value} already exists on shopping list {listId.Value}";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.ItemAlreadyOnShoppingList;
}