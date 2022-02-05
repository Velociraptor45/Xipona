using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;

public class StoreItemSectionNotFoundReason : IReason
{
    public StoreItemSectionNotFoundReason(SectionId sectionId)
    {
        Message = $"Store item section {sectionId.Value} not found";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.StoreItemSectionNotFound;
}