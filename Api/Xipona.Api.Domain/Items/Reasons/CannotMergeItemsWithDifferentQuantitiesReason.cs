using Xipona.Api.Domain.Common.Reasons;

namespace Xipona.Api.Domain.Items.Reasons;
public class CannotMergeItemsWithDifferentQuantitiesReason : IReason
{
    public string Message => "Cannot merge items with different quantities";
    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotMergeItemsWithDifferentQuantities;
}
