using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Reasons;

public class CannotMakeDeletedItemPermanentReason : IReason
{
    public CannotMakeDeletedItemPermanentReason(ItemId id)
    {
        Message = $"Cannot make deleted item ({id.Value}) permanent";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotMakeDeletedItemPermanent;
}