using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.Items.Models;

namespace Xipona.Api.Domain.Items.Reasons;

public class CannotModifyDeletedItemReason : IReason
{
    public CannotModifyDeletedItemReason(ItemId id)
    {
        Message = $"Cannot modify deleted item ({id.Value})";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotModifyDeletedItem;
}