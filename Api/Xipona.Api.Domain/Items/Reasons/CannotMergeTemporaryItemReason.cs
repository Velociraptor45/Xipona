using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.Items.Models;

namespace Xipona.Api.Domain.Items.Reasons;
public class CannotMergeTemporaryItemReason : IReason
{
    public CannotMergeTemporaryItemReason(ItemId itemId)
    {
        Message = $"Item '{itemId}' is temporary and, thus, cannot be merged.";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotMergeTemporaryItem;
}
