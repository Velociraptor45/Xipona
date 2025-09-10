using Xipona.Api.Domain.Common.Reasons;

namespace Xipona.Api.Domain.Items.Reasons;
public class CannotMergeItemsWithDifferentManufacturersReason : IReason
{
    public string Message => "Cannot merge items with different manufacturers";
    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotMergeItemsWithDifferentManufacturers;
}
