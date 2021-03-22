using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.ErrorReasons
{
    public class ItemAlreadyInSectionReason : IReason
    {
        public ItemAlreadyInSectionReason(ItemId shoppingListItemId, ShoppingListSectionId sectionId)
        {
            Message = $"Item {shoppingListItemId} is already in section {sectionId}";
        }

        public string Message { get; }

        public ErrorReasonCode ErrorCode => ErrorReasonCode.ItemAlreadyInSection;
    }
}