using ProjectHermes.Xipona.Api.Domain.Common.Reasons;

namespace ProjectHermes.Xipona.Api.Domain.Items.Reasons;

public class InvalidQuantityReason : IReason
{
    public InvalidQuantityReason(float quantity)
    {
        Message = $"Quantity must be greater than 0 but was {quantity}";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.InvalidQuantity;
}