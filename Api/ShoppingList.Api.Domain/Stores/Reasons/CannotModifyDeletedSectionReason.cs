using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Reasons;

public class CannotModifyDeletedSectionReason : IReason
{
    public CannotModifyDeletedSectionReason(SectionId sectionId)
    {
        Message = $"Cannot modify deleted section ({sectionId.Value})";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotModifyDeletedSection;
}