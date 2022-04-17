using ProjectHermes.ShoppingList.Api.Domain.Common.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Reasons;

public class ShoppingListNotFoundReason : IReason
{
    public ShoppingListNotFoundReason(ShoppingListId id)
    {
        Message = $"Shopping list {id.Value} not found.";
    }

    public ShoppingListNotFoundReason(StoreId id)
    {
        Message = $"No active shopping list for store {id.Value} found.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.ShoppingListNotFound;
}