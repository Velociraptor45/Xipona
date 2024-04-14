using Fluxor;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States.Comparer;
using ProjectHermes.Xipona.Frontend.Redux.Stores.Actions.Editor;

namespace ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Reducers;

public static class ShoppingListReducer
{
    [ReducerMethod(typeof(ShoppingListEnteredAction))]
    public static ShoppingListState OnShoppingListEntered(ShoppingListState state)
    {
        return state with
        {
            SelectedStoreId = Guid.Empty,
            ItemsInBasketVisible = true,
            EditModeActive = false,
            ShoppingList = null,
            SearchBar = state.SearchBar with
            {
                Input = string.Empty,
                Results = new List<SearchItemForShoppingListResult>()
            }
        };
    }

    [ReducerMethod]
    public static ShoppingListState OnLoadQuantityTypesFinished(ShoppingListState state,
        LoadQuantityTypesFinishedAction action)
    {
        return state with { QuantityTypes = action.QuantityTypes.ToList() };
    }

    [ReducerMethod]
    public static ShoppingListState OnLoadQuantityTypesInPacketFinished(ShoppingListState state,
        LoadQuantityTypesInPacketFinishedAction action)
    {
        return state with { QuantityTypesInPacket = action.QuantityTypesInPacket.ToList() };
    }

    [ReducerMethod]
    public static ShoppingListState OnLoadAllActiveStoresFinished(ShoppingListState state,
        LoadAllActiveStoresFinishedAction action)
    {
        return state with
        {
            Stores = new AllActiveStores(action.Stores.OrderBy(s => s.Name).ToList())
        };
    }

    [ReducerMethod]
    public static ShoppingListState OnSelectedStoreChanged(ShoppingListState state,
        SelectedStoreChangedAction action)
    {
        return state with
        {
            SelectedStoreId = action.StoreId,
            SearchBar = state.SearchBar with
            {
                Input = string.Empty,
                Results = new List<SearchItemForShoppingListResult>()
            }
        };
    }

    [ReducerMethod]
    public static ShoppingListState OnLoadShoppingListFinished(ShoppingListState state,
        LoadShoppingListFinishedAction action)
    {
        return state with
        {
            ItemsInBasketVisible = true,
            ShoppingList = action.ShoppingList
        };
    }

    [ReducerMethod(typeof(ResetEditModeAction))]
    public static ShoppingListState OnResetEditMode(ShoppingListState state)
    {
        return state with
        {
            EditModeActive = false
        };
    }

    [ReducerMethod(typeof(ToggleItemsInBasketVisibleAction))]
    public static ShoppingListState OnToggleItemsInBasketVisible(ShoppingListState state)
    {
        return state with { ItemsInBasketVisible = !state.ItemsInBasketVisible };
    }

    [ReducerMethod(typeof(ToggleEditModeAction))]
    public static ShoppingListState OnToggleEditMode(ShoppingListState state)
    {
        return state with { EditModeActive = !state.EditModeActive };
    }

    [ReducerMethod]
    public static ShoppingListState OnToggleShoppingListSectionExpansion(ShoppingListState state,
        ToggleShoppingListSectionExpansionAction action)
    {
        if (state.ShoppingList is null)
            return state;

        var sections = new List<ShoppingListSection>(state.ShoppingList!.Sections);
        var section = sections.FirstOrDefault(s => s.Id == action.SectionId);
        if (section == null)
            return state;

        var sectionIndex = sections.IndexOf(section);
        sections[sectionIndex] = section with { IsExpanded = !section.IsExpanded };

        return state with
        {
            ShoppingList = state.ShoppingList with
            {
                Sections = new SortedSet<ShoppingListSection>(sections, new SortingIndexComparer())
            }
        };
    }

    [ReducerMethod(typeof(SaveStoreFinishedAction))]
    public static ShoppingListState OnSaveStoreFinished(ShoppingListState state)
    {
        return ClearStores(state);
    }

    [ReducerMethod(typeof(DeleteStoreFinishedAction))]
    public static ShoppingListState OnDeleteStoreFinished(ShoppingListState state)
    {
        return ClearStores(state);
    }

    private static ShoppingListState ClearStores(ShoppingListState state)
    {
        return state with
        {
            Stores = state.Stores with
            {
                Stores = new List<ShoppingListStore>(0)
            }
        };
    }
}