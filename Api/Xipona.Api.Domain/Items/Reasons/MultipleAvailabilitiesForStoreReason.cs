using ProjectHermes.Xipona.Api.Domain.Common.Reasons;

namespace ProjectHermes.Xipona.Api.Domain.Items.Reasons;

public class MultipleAvailabilitiesForStoreReason : IReason
{
    public string Message => "Multiple availabilities for one store were provided.";

    public ErrorReasonCode ErrorCode => ErrorReasonCode.MultipleAvailabilitiesForStore;
}