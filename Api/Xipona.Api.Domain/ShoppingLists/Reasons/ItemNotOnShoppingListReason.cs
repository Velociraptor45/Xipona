using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.Xipona.Api.Domain.ShoppingLists.Reasons;

public class ItemNotOnShoppingListReason : IReason
{
    public ItemNotOnShoppingListReason(ShoppingListId shoppingListId, ItemId shoppingListItemId,
        ItemTypeId? itemTypeId = null)
    {
        Message = itemTypeId is null
            ? $"Item {shoppingListItemId} is not on shopping list {shoppingListId.Value}."
            : $"Item {shoppingListItemId} with type {itemTypeId} is not on shopping list {shoppingListId.Value}.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.ItemNotOnShoppingList;
}