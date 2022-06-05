using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.ErrorReasons;

public class ItemAlreadyInSectionReason : IReason
{
    public ItemAlreadyInSectionReason(ItemId shoppingListItemId, SectionId sectionId)
    {
        Message = $"Item {shoppingListItemId} is already in section {sectionId}";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.ItemAlreadyInSection;
}