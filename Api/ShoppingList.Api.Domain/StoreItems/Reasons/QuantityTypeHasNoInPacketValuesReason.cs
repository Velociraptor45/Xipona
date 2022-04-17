using ProjectHermes.ShoppingList.Api.Domain.Common.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Reasons;

public class QuantityTypeHasNoInPacketValuesReason : IReason
{
    public QuantityTypeHasNoInPacketValuesReason(QuantityType type)
    {
        Message = $"Quantity type {type} has no in-packet values.";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.QuantityTypeHasNoInPacketValues;
}