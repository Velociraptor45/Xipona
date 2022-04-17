using ProjectHermes.ShoppingList.Api.Domain.Common.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Shared;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Reasons;

public class ItemNotFoundReason : IReason
{
    public ItemNotFoundReason(ItemId id)
    {
        Message = $"Item {id} not found.";
    }

    public ItemNotFoundReason(TemporaryItemId id)
    {
        Message = $"Item {id} not found.";
    }

    public ItemNotFoundReason(OfflineTolerantItemId id)
    {
        string s = id.IsActualId ? id.ActualId!.Value.ToString() : id.OfflineId!.Value.ToString();
        Message = $"Item {s} not found.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.ItemNotFound;
}