namespace ProjectHermes.Xipona.Api.Domain.Common.Reasons;

public class GeneralSettingsNotLoadedReason : IReason
{
    public string Message => "General settings are not loaded. This is a core application error. Try restarting the API";
    public ErrorReasonCode ErrorCode => ErrorReasonCode.GeneralSettingsNotLoaded;
}
