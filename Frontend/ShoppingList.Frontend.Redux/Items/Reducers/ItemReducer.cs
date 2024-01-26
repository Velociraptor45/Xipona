using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.Actions.Search;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Stores.Actions.Editor;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Items.Reducers;

public static class ItemReducer
{
    [ReducerMethod(typeof(SearchItemsStartedAction))]
    public static ItemState OnSearchItemStarted(ItemState state)
    {
        return state with
        {
            Search = state.Search with
            {
                IsLoadingSearchResults = true
            }
        };
    }

    [ReducerMethod]
    public static ItemState OnSearchItemFinished(ItemState state, SearchItemsFinishedAction action)
    {
        return state with
        {
            Search = state.Search with
            {
                IsLoadingSearchResults = false,
                TriggeredAtLeastOnce = true,
                SearchResults = action.SearchResults.OrderBy(r => r.Name).ToList()
            }
        };
    }

    [ReducerMethod]
    public static ItemState OnLoadQuantityTypesFinished(ItemState state, LoadQuantityTypesFinishedAction action)
    {
        return state with { QuantityTypes = action.QuantityTypes };
    }

    [ReducerMethod]
    public static ItemState OnLoadQuantityTypesInPacketFinished(ItemState state,
        LoadQuantityTypesInPacketFinishedAction action)
    {
        return state with { QuantityTypesInPacket = action.QuantityTypesInPacket };
    }

    [ReducerMethod]
    public static ItemState OnLoadActiveStoresFinished(ItemState state, LoadActiveStoresFinishedAction action)
    {
        return state with { Stores = action.Stores };
    }

    [ReducerMethod(typeof(SaveStoreFinishedAction))]
    public static ItemState OnSaveStoreFinished(ItemState state)
    {
        return ClearStores(state);
    }

    [ReducerMethod(typeof(DeleteStoreFinishedAction))]
    public static ItemState OnDeleteStoreFinished(ItemState state)
    {
        return ClearStores(state);
    }

    private static ItemState ClearStores(ItemState state)
    {
        return state with
        {
            Stores = state.Stores with
            {
                Stores = new List<ItemStore>(0)
            }
        };
    }
}