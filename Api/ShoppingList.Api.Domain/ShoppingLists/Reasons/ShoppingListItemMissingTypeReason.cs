using ProjectHermes.ShoppingList.Api.Domain.Common.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Reasons;

public class ShoppingListItemMissingTypeReason : IReason
{
    public ShoppingListItemMissingTypeReason(ItemId itemId)
    {
        Message = $"Item {itemId} on shopping list is missing a type. Items with types cannot be on a shopping list without a type.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.ShoppingListItemMissingType;
}