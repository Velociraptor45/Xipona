using Xipona.Api.Domain.Common.Reasons;

namespace Xipona.Api.Domain.Items.Reasons;
public class CannotMergeItemWithItselfReason : IReason
{
    public string Message => "Cannot merge an item with itself.";
    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotMergeItemWithItself;
}
