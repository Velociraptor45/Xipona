using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;

public class QuantityTypeHasNoInPacketValuesReason : IReason
{
    public QuantityTypeHasNoInPacketValuesReason(QuantityType type)
    {
        Message = $"Quantity type {type} has no in-packet values.";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.QuantityTypeHasNoInPacketValues;
}