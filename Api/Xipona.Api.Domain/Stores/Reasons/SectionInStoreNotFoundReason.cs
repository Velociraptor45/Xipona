using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.Stores.Reasons;

public class SectionInStoreNotFoundReason : IReason
{
    public SectionInStoreNotFoundReason(SectionId sectionId, StoreId storeId)
    {
        Message = $"Section {sectionId.Value} wasn't found in store {storeId.Value}.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.SectionInStoreNotFound;
}