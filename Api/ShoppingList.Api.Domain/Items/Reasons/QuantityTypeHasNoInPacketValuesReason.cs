using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Reasons;

public class QuantityTypeHasNoInPacketValuesReason : IReason
{
    public QuantityTypeHasNoInPacketValuesReason(QuantityType type)
    {
        Message = $"Quantity type {type} has no in-packet values.";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.QuantityTypeHasNoInPacketValues;
}