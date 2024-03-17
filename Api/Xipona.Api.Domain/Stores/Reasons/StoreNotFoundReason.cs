using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.Stores.Reasons;

public class StoreNotFoundReason : IReason
{
    public StoreNotFoundReason(StoreId id)
    {
        Message = $"Store {id.Value} not found.";
    }

    public StoreNotFoundReason(SectionId sectionId)
    {
        Message = $"No store for section {sectionId.Value} found.";
    }

    public StoreNotFoundReason(IEnumerable<StoreId> storeIds)
    {
        Message = $"No store found for store ids {string.Join(", ", storeIds.Select(x => x.Value))}.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.StoreNotFound;
}