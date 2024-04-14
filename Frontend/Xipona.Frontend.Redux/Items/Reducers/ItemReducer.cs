using Fluxor;
using ProjectHermes.Xipona.Frontend.Redux.Items.Actions;
using ProjectHermes.Xipona.Frontend.Redux.Items.Actions.Search;
using ProjectHermes.Xipona.Frontend.Redux.Items.States;
using ProjectHermes.Xipona.Frontend.Redux.Stores.Actions.Editor;

namespace ProjectHermes.Xipona.Frontend.Redux.Items.Reducers;

public static class ItemReducer
{
    [ReducerMethod]
    public static ItemState OnItemSearchInputChanged(ItemState state, ItemSearchInputChangedAction action)
    {
        return state with
        {
            Search = state.Search with
            {
                Input = action.Input
            }
        };
    }

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
        return state with
        {
            QuantityTypes = action.QuantityTypes,
            Initialization = state.Initialization with
            {
                QuantityTypesLoaded = true
            }
        };
    }

    [ReducerMethod]
    public static ItemState OnLoadQuantityTypesInPacketFinished(ItemState state,
        LoadQuantityTypesInPacketFinishedAction action)
    {
        return state with
        {
            QuantityTypesInPacket = action.QuantityTypesInPacket,
            Initialization = state.Initialization with
            {
                QuantityTypesInPacketLoaded = true
            }
        };
    }

    [ReducerMethod]
    public static ItemState OnLoadActiveStoresFinished(ItemState state, LoadActiveStoresFinishedAction action)
    {
        return state with
        {
            Stores = action.Stores,
            Initialization = state.Initialization with
            {
                StoresLoaded = true
            }
        };
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