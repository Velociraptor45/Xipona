using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States.Comparer;

namespace ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Reducers;

public static class ShoppingListReducer
{
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
                IsActive = false,
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
            EditModeActive = false,
            ItemsInBasketVisible = true,
            ShoppingList = action.ShoppingList
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
}