using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;

public class QuantityTypeHasInPacketValuesReason : IReason
{
    public QuantityTypeHasInPacketValuesReason(QuantityType type)
    {
        Message = $"Quantity type {type} has in-packet values.";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.QuantityTypeHasInPacketValues;
}