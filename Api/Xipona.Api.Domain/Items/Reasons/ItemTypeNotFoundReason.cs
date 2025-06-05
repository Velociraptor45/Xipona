using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.Items.Models;

namespace Xipona.Api.Domain.Items.Reasons;

public class ItemTypeNotFoundReason : IReason
{
    public ItemTypeNotFoundReason(ItemId itemId, ItemTypeId itemTypeId)
    {
        Message = $"Item {itemId.Value} does not contain item type {itemTypeId.Value}.";
    }

    public ItemTypeNotFoundReason(ItemTypeId itemTypeId)
    {
        Message = $"Item type {itemTypeId.Value} not found.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.ItemTypeNotFound;
}