using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;

public class CannotRemoveAllTypesFromItemWithTypesReason : IReason
{
    public CannotRemoveAllTypesFromItemWithTypesReason(ItemId itemId)
    {
        Message = $"You cannot remove all types from item with types (id: {itemId.Value}).";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotRemoveAllTypesFromItemWithTypes;
}