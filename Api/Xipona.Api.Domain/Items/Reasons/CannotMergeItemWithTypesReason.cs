using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.Items.Models;

namespace Xipona.Api.Domain.Items.Reasons;
public class CannotMergeItemWithTypesReason : IReason
{
    public CannotMergeItemWithTypesReason(ItemId itemId)
    {
        Message = $"Item '{itemId}' has types and, thus, cannot be merged";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotMergeItemWithTypes;
}
