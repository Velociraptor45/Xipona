using ProjectHermes.Xipona.Api.Domain.Common.Reasons;

namespace ProjectHermes.Xipona.Api.Domain.Items.Reasons;

public class CannotModifyItemWithoutAvailabilitiesReason : IReason
{
    public CannotModifyItemWithoutAvailabilitiesReason()
    {
        Message = "Cannot modify item without availabilities";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotModifyItemWithoutAvailabilities;
}