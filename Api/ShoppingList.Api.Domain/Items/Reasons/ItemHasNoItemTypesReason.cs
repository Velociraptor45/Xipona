using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Reasons;

public class ItemHasNoItemTypesReason : IReason
{
    public ItemHasNoItemTypesReason(ItemId itemId)
    {
        Message = $"Item {itemId.Value} has no types";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.ItemHasNoItemTypes;
}