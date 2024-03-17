using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.Reasons;

public class CannotRemoveAllTypesFromItemWithTypesReason : IReason
{
    public CannotRemoveAllTypesFromItemWithTypesReason(ItemId itemId)
    {
        Message = $"You cannot remove all types from item with types (id: {itemId.Value}).";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotRemoveAllTypesFromItemWithTypes;
}