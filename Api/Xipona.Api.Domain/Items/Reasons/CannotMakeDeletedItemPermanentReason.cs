using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.Reasons;

public class CannotMakeDeletedItemPermanentReason : IReason
{
    public CannotMakeDeletedItemPermanentReason(ItemId id)
    {
        Message = $"Cannot make deleted item ({id.Value}) permanent";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotMakeDeletedItemPermanent;
}