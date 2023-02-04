using Fluxor;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Stores.States;
public record StoreState(
    IReadOnlyCollection<StoreSearchResult> SearchResults,
    StoreEditor Editor);

public class StoreFeatureState : Feature<StoreState>
{
    public override string GetName()
    {
        return nameof(StoreState);
    }

    protected override StoreState GetInitialState()
    {
        return new StoreState(
            new List<StoreSearchResult>(0),
            new StoreEditor(null));
    }
}