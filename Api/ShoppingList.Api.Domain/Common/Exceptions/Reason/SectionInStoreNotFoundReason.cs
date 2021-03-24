using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason
{
    public class SectionInStoreNotFoundReason : IReason
    {
        public SectionInStoreNotFoundReason(SectionId sectionId, StoreId storeId)
        {
            Message = $"Section {sectionId} wasn't found in store {storeId}.";
        }

        public string Message { get; }

        public ErrorReasonCode ErrorCode => ErrorReasonCode.SectionInStoreNotFound;
    }
}