using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Reasons;

public class OldAndNewSectionNotInSameStoreReason : IReason
{
    public OldAndNewSectionNotInSameStoreReason(SectionId oldSectionId, SectionId newSectionId)
    {
        Message = $"Old section {oldSectionId.Value} not in same store as new section {newSectionId.Value}";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.OldAndNewSectionNotInSameStore;
}