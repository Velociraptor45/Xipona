using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Reasons;
public class CannotModifyItemWithTypesAsItemReason : IReason
{
    public CannotModifyItemWithTypesAsItemReason(ItemId itemId)
    {
        Message = $"Item {itemId.Value} has types and thus can't be modified as item.";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotModifyItemWithTypesAsItem;
}
