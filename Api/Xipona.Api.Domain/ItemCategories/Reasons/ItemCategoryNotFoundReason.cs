using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.ItemCategories.Models;

namespace Xipona.Api.Domain.ItemCategories.Reasons;

public class ItemCategoryNotFoundReason : IReason
{
    public ItemCategoryNotFoundReason(ItemCategoryId id)
    {
        Message = $"Item category {id.Value} not found.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.ItemCategoryNotFound;
}