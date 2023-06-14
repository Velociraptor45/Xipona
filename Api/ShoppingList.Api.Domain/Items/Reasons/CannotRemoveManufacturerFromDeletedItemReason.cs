using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Reasons;

public class CannotRemoveManufacturerFromDeletedItemReason : IReason
{
    public CannotRemoveManufacturerFromDeletedItemReason(ItemId id)
    {
        Message = $"Cannot remove manufacturer from deleted item ({id.Value})";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotRemoveManufacturerFromDeletedItem;
}