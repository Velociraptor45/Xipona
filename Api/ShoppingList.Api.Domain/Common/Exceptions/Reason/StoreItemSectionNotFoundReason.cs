using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason
{
    public class StoreItemSectionNotFoundReason : IReason
    {
        public StoreItemSectionNotFoundReason(StoreItemSectionId sectionId)
        {
            Message = $"Store item section {sectionId.Value} not found";
        }

        public string Message { get; }

        public ErrorReasonCode ErrorCode => ErrorReasonCode.StoreItemSectionNotFound;
    }
}