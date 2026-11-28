using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.Items.Models;

namespace Xipona.Api.Domain.Items.Reasons;

public class ItemTypeNotPartOfItemReason : IReason
{
    public ItemTypeNotPartOfItemReason(ItemId itemId, ItemTypeId itemTypeId)
    {
        Message = $"Item type {itemTypeId.Value} is not part of item {itemId.Value}";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.ItemTypeNotPartOfItem;
}