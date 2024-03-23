using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.Reasons;

public class CannotUpdateItemWithTypesAsItemReason : IReason
{
    public CannotUpdateItemWithTypesAsItemReason(ItemId itemId)
    {
        Message = $"Item {itemId.Value} has types and thus can't be updated as an item without types.";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotUpdateItemWithTypesAsItem;
}