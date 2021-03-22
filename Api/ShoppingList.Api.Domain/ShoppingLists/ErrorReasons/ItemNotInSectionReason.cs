using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.ErrorReasons
{
    public class ItemNotInSectionReason : IReason
    {
        public ItemNotInSectionReason(ItemId shoppingListItemId, ShoppingListSectionId sectionId)
        {
            Message = $"Item {shoppingListItemId} isn't in section {sectionId}";
        }

        public string Message { get; }

        public ErrorReasonCode ErrorCode => ErrorReasonCode.ItemNotInSection;
    }
}