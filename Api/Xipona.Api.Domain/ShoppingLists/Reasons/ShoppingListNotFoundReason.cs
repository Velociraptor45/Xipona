using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.ShoppingLists.Models;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Domain.ShoppingLists.Reasons;

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