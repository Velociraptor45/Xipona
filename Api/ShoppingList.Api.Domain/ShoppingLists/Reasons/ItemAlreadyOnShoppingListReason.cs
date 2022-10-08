using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Reasons;

public class ItemAlreadyOnShoppingListReason : IReason
{
    public ItemAlreadyOnShoppingListReason(ItemId itemId, ShoppingListId listId)
    {
        Message = $"Item {itemId.Value} already exists on shopping list {listId.Value}";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.ItemAlreadyOnShoppingList;
}