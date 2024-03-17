using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Shared;

namespace ProjectHermes.Xipona.Api.Domain.Items.Reasons;

public class ItemNotFoundReason : IReason
{
    public ItemNotFoundReason(IEnumerable<ItemId> itemIds)
    {
        Message = $"Items '{string.Join(", ", itemIds.Select(i => i.Value))}' not found.";
    }

    public ItemNotFoundReason(ItemId id)
    {
        Message = $"Item {id.Value} not found.";
    }

    public ItemNotFoundReason(TemporaryItemId id)
    {
        Message = $"Item {id.Value} not found.";
    }

    public ItemNotFoundReason(OfflineTolerantItemId id)
    {
        string s = id.IsActualId ? id.ActualId!.Value.ToString() : id.OfflineId!.Value.ToString();
        Message = $"Item {s} not found.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.ItemNotFound;
}