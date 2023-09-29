using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Reasons;

public class CannotTransferDeletedItemReason : IReason
{
    public CannotTransferDeletedItemReason(ItemId id)
    {
        Message = $"Cannot transfer deleted item ({id.Value})";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotTransferDeletedItem;
}