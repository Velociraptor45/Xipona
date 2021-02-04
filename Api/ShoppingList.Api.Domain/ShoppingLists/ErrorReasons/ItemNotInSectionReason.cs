using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.ErrorReasons
{
    public class ItemNotInSectionReason : IReason
    {
        public ItemNotInSectionReason(ShoppingListItemId shoppingListItemId, ShoppingListSectionId sectionId)
        {
            Message = $"Item {shoppingListItemId} isn't in section {sectionId}";
        }

        public string Message { get; }

        public ErrorReasonCode ErrorCode => ErrorReasonCode.ItemNotInSection;
    }
}