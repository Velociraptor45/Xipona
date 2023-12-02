using Fluxor;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Shared.States;
public record SharedState(bool IsMobile);

public class SharedFeatureState : Feature<SharedState>
{
    public override string GetName()
    {
        return nameof(SharedState);
    }

    protected override SharedState GetInitialState()
    {
        return new SharedState(false);
    }
}