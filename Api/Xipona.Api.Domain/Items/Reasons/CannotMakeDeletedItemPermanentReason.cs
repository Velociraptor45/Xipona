using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.Items.Models;

namespace Xipona.Api.Domain.Items.Reasons;

public class CannotMakeDeletedItemPermanentReason : IReason
{
    public CannotMakeDeletedItemPermanentReason(ItemId id)
    {
        Message = $"Cannot make deleted item ({id.Value}) permanent";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotMakeDeletedItemPermanent;
}