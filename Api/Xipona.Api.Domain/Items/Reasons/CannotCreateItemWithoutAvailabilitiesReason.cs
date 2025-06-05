using Xipona.Api.Domain.Common.Reasons;

namespace Xipona.Api.Domain.Items.Reasons;

public class CannotCreateItemWithoutAvailabilitiesReason : IReason
{
    public string Message => "Cannot create item without availabilities";
    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotCreateItemWithoutAvailabilities;
}