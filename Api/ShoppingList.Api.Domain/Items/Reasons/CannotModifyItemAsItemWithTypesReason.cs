using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Reasons;

public class CannotModifyItemAsItemWithTypesReason : IReason
{
    public CannotModifyItemAsItemWithTypesReason(ItemId itemId)
    {
        Message = $"Item {itemId.Value} does not have types and thus can't be modified with types.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotModifyItemAsItemWithTypes;
}