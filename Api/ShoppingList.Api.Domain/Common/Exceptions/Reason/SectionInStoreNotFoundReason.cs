using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;

public class SectionInStoreNotFoundReason : IReason
{
    public SectionInStoreNotFoundReason(SectionId sectionId, StoreId storeId)
    {
        Message = $"Section {sectionId.Value} wasn't found in store {storeId.Value}.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.SectionInStoreNotFound;
}