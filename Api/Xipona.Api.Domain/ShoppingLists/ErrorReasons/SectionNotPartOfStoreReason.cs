using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.ShoppingLists.ErrorReasons;

public class SectionNotPartOfStoreReason : IReason
{
    public SectionNotPartOfStoreReason(SectionId sectionId, StoreId storeId)
    {
        Message = $"Section {sectionId} is not part of store {storeId}.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.SectionNotPartOfStore;
}