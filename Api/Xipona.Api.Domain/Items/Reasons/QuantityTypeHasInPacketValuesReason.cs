using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.Reasons;

public class QuantityTypeHasInPacketValuesReason : IReason
{
    public QuantityTypeHasInPacketValuesReason(QuantityType type)
    {
        Message = $"Quantity type {type} has in-packet values.";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.QuantityTypeHasInPacketValues;
}