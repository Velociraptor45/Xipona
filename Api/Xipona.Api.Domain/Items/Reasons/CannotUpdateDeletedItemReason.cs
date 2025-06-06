using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.Items.Models;

namespace Xipona.Api.Domain.Items.Reasons;

public class CannotUpdateDeletedItemReason : IReason
{
    public CannotUpdateDeletedItemReason(ItemId id)
    {
        Message = $"Cannot update deleted item ({id.Value})";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotUpdateDeletedItem;
}