using ProjectHermes.Xipona.Api.Domain.Common.Reasons;

namespace ProjectHermes.Xipona.Api.Domain.Items.Reasons;

public class CannotModifyItemTypeWithoutAvailabilitiesReason : IReason
{
    public CannotModifyItemTypeWithoutAvailabilitiesReason()
    {
        Message = "Cannot modify item type without availabilities";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotModifyItemTypeWithoutAvailabilities;
}