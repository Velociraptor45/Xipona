using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.Reasons;

public class ItemHasNoItemTypesReason : IReason
{
    public ItemHasNoItemTypesReason(ItemId itemId)
    {
        Message = $"Item {itemId.Value} has no types";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.ItemHasNoItemTypes;
}