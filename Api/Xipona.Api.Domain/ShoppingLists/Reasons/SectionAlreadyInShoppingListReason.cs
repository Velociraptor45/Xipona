using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.ShoppingLists.Models;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Domain.ShoppingLists.Reasons;

public class SectionAlreadyInShoppingListReason : IReason
{
    public SectionAlreadyInShoppingListReason(ShoppingListId shoppingListId, SectionId sectionId)
    {
        Message = $"Section {sectionId} is already part of shopping list {shoppingListId}.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.SectionAlreadyInShoppingList;
}