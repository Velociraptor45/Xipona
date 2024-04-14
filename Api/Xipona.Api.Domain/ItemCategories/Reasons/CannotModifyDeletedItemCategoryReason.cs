using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;

namespace ProjectHermes.Xipona.Api.Domain.ItemCategories.Reasons;

public class CannotModifyDeletedItemCategoryReason : IReason
{
    public CannotModifyDeletedItemCategoryReason(ItemCategoryId id)
    {
        Message = $"Cannot modify item category with id '{id.Value}' because it is deleted";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotModifyDeletedItemCategory;
}