using ProjectHermes.ShoppingList.Api.Domain.Common.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Reasons;

public class ItemNotOnShoppingListReason : IReason
{
    public ItemNotOnShoppingListReason(ShoppingListId shoppingListId, ItemId shoppingListItemId)
    {
        Message = $"Item {shoppingListItemId} is not on shopping list {shoppingListId.Value}.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.ItemNotOnShoppingList;
}