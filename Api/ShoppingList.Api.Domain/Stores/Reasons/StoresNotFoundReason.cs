using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Reasons;

public class StoresNotFoundReason : IReason
{
    public StoresNotFoundReason(IEnumerable<StoreId> ids)
    {
        Message = $"Stores {string.Join(", ", ids.Select(id => id.Value))} not found.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.StoresNotFound;
}