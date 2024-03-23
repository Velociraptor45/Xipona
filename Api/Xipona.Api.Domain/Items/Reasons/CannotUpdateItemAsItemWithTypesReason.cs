using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.Reasons;

public class CannotUpdateItemAsItemWithTypesReason : IReason
{
    public CannotUpdateItemAsItemWithTypesReason(ItemId itemId)
    {
        Message = $"Item {itemId.Value} does not have types and thus can't be updated with types.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotUpdateItemAsItemWithTypes;
}