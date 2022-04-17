using ProjectHermes.ShoppingList.Api.Domain.Common.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Reasons;

public class SectionAlreadyInShoppingListReason : IReason
{
    public SectionAlreadyInShoppingListReason(ShoppingListId shoppingListId, SectionId sectionId)
    {
        Message = $"Section {sectionId} is already part of shopping list {shoppingListId}.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.SectionAlreadyInShoppingList;
}