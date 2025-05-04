using Fluxor;

namespace ProjectHermes.Xipona.Frontend.Redux.Shared.States;

public record SharedState(UserInfo? User, bool IsMobile, bool IsOnline, bool IsRetryOngoing, Settings Settings);

public class SharedFeatureState : Feature<SharedState>
{
    public override string GetName()
    {
        return nameof(SharedState);
    }

    protected override SharedState GetInitialState()
    {
        return new SharedState(null, false, true, false, new(new(null!), null, [], false));
    }
}