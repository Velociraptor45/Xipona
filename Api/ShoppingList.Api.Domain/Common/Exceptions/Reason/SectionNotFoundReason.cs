using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;

public class SectionNotFoundReason : IReason
{
    public SectionNotFoundReason(SectionId sectionId)
    {
        Message = $"Section {sectionId} not found. Are you looking in the wrong store?";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.SectionNotFound;
}