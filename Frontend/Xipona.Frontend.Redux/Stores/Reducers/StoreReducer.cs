using Fluxor;
using ProjectHermes.Xipona.Frontend.Redux.Stores.Actions;
using ProjectHermes.Xipona.Frontend.Redux.Stores.Actions.Editor;
using ProjectHermes.Xipona.Frontend.Redux.Stores.States;

namespace ProjectHermes.Xipona.Frontend.Redux.Stores.Reducers;

public static class StoreReducer
{
    [ReducerMethod]
    public static StoreState OnLoadStoresOverviewFinished(StoreState state, LoadStoresOverviewFinishedAction action)
    {
        return state with
        {
            SearchResults = action.SearchResults.OrderBy(r => r.Name).ToList()
        };
    }

    [ReducerMethod(typeof(StorePageInitializedAction))]
    public static StoreState OnStorePageInitialized(StoreState state)
    {
        return state with
        {
            SearchResults = new List<StoreSearchResult>(0),
            Editor = state.Editor with
            {
                Store = null,
                IsSaving = false
            }
        };
    }
}