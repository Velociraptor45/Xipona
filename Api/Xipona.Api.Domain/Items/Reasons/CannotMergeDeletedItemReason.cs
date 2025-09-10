using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.Items.Models;

namespace Xipona.Api.Domain.Items.Reasons;
public class CannotMergeDeletedItemReason : IReason
{
    public CannotMergeDeletedItemReason(ItemId itemId)
    {
        Message = $"Item '{itemId}' is deleted and, thus, cannot be merged.";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotMergeDeletedItem;
}
