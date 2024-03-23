using ProjectHermes.Xipona.Api.Domain.Common.Reasons;

namespace ProjectHermes.Xipona.Api.Domain.Items.Reasons;

public class PriceNotValidReason : IReason
{
    public PriceNotValidReason()
    {
        Message = "Price must be greater 0.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.PriceNotValid;
}