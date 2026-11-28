using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.Items.Models;

namespace Xipona.Api.Domain.Items.Reasons;

public class CannotTransferDeletedItemReason : IReason
{
    public CannotTransferDeletedItemReason(ItemId id)
    {
        Message = $"Cannot transfer deleted item ({id.Value})";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotTransferDeletedItem;
}