using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.Items.Models;

namespace Xipona.Api.Domain.Items.Reasons;

public class ItemHasNoItemTypesReason : IReason
{
    public ItemHasNoItemTypesReason(ItemId itemId)
    {
        Message = $"Item {itemId.Value} has no types";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.ItemHasNoItemTypes;
}