using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Model;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.ErrorReasons
{
    public class SectionNotPartOfStoreReason : IReason
    {
        public SectionNotPartOfStoreReason(ShoppingListSectionId sectionId, StoreId storeId)
        {
            Message = $"Section {sectionId} is not part of store {storeId}.";
        }

        public string Message { get; }

        public ErrorReasonCode ErrorCode => ErrorReasonCode.SectionNotPartOfStore;
    }
}