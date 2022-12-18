using Fluxor;
using ProjectHermes.ShoppingList.Frontend.WebApp.Store.Manufacturers.Actions;
using ProjectHermes.ShoppingList.Frontend.WebApp.Store.Manufacturers.States;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Store.Manufacturers.Reducers;
public static class ManufacturerReducer
{
    [ReducerMethod(typeof(SearchManufacturersStartedAction))]
    public static ManufacturerState OnSearch(ManufacturerState state)
    {
        return state with
        {
            IsLoadingSearchResults = true
        };
    }

    [ReducerMethod]
    public static ManufacturerState OnSearch(ManufacturerState state, SearchManufacturersFinishedAction action)
    {
        return state with
        {
            IsLoadingSearchResults = false,
            SearchResults = action.SearchResults
        };
    }
}
