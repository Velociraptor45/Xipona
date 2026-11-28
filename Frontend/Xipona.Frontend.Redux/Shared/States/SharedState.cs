using Fluxor;

namespace Xipona.Frontend.Redux.Shared.States;

public record SharedState(UserInfo? User, bool IsMobile, bool IsOnline, bool IsRetryOngoing, Settings Settings)
{
    public string GetCurrencySymbol()
    {
        return Settings.GeneralSettings?.Currency.Symbol ?? string.Empty;
    }
}

public class SharedFeatureState : Feature<SharedState>
{
    public override string GetName()
    {
        return nameof(SharedState);
    }

    protected override SharedState GetInitialState()
    {
        return new SharedState(null, false, true, false, new(null, null, null, false, false));
    }
}