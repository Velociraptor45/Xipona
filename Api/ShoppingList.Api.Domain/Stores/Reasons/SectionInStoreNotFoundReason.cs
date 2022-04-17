using ProjectHermes.ShoppingList.Api.Domain.Common.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Reasons;

public class SectionInStoreNotFoundReason : IReason
{
    public SectionInStoreNotFoundReason(SectionId sectionId, StoreId storeId)
    {
        Message = $"Section {sectionId.Value} wasn't found in store {storeId.Value}.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.SectionInStoreNotFound;
}