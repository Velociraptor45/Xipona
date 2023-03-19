using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Reasons;

public class CannotModifyDeletedItemTypeReason : IReason
{
    public CannotModifyDeletedItemTypeReason(ItemTypeId id)
    {
        Message = $"Cannot modify deleted item type {id.Value}";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotModifyDeletedItemType;
}