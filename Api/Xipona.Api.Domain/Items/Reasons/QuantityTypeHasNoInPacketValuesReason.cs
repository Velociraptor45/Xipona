using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.Items.Models;

namespace Xipona.Api.Domain.Items.Reasons;

public class QuantityTypeHasNoInPacketValuesReason : IReason
{
    public QuantityTypeHasNoInPacketValuesReason(QuantityType type)
    {
        Message = $"Quantity type {type} has no in-packet values.";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.QuantityTypeHasNoInPacketValues;
}