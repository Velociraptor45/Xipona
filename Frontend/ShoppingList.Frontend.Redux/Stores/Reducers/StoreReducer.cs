using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Redux.Stores.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.Stores.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Stores.Reducers;

public static class StoreReducer
{
    [ReducerMethod]
    public static StoreState OnLoadStoresOverviewFinished(StoreState state, LoadStoresOverviewFinishedAction action)
    {
        return state with { SearchResults = action.SearchResults };
    }
}