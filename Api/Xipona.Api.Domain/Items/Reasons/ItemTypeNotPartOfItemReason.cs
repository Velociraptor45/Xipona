using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.Reasons;

public class ItemTypeNotPartOfItemReason : IReason
{
    public ItemTypeNotPartOfItemReason(ItemId itemId, ItemTypeId itemTypeId)
    {
        Message = $"Item type {itemTypeId.Value} is not part of item {itemId.Value}";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.ItemTypeNotPartOfItem;
}