using Xipona.Api.Domain.Common.Reasons;

namespace Xipona.Api.Domain.Items.Reasons;
public class CannotMergeItemsWithDifferentItemCategoriesReason : IReason
{
    public string Message => "Cannot merge items with different item categories";
    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotMergeItemsWithDifferentItemCategories;
}
