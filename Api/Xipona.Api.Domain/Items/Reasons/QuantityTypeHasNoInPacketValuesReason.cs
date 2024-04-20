using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.Reasons;

public class QuantityTypeHasNoInPacketValuesReason : IReason
{
    public QuantityTypeHasNoInPacketValuesReason(QuantityType type)
    {
        Message = $"Quantity type {type} has no in-packet values.";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.QuantityTypeHasNoInPacketValues;
}