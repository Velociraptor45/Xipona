using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Reasons;

public class QuantityTypeHasInPacketValuesReason : IReason
{
    public QuantityTypeHasInPacketValuesReason(QuantityType type)
    {
        Message = $"Quantity type {type} has in-packet values.";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.QuantityTypeHasInPacketValues;
}