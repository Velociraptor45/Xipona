using Fluxor;

namespace ProjectHermes.Xipona.Frontend.Redux.Shared.States;
public record SharedState(bool IsMobile, bool IsOnline);

public class SharedFeatureState : Feature<SharedState>
{
    public override string GetName()
    {
        return nameof(SharedState);
    }

    protected override SharedState GetInitialState()
    {
        return new SharedState(false, true);
    }
}