using Fluxor;
using ShoppingList.Frontend.Redux.ShoppingList.Actions.SearchBar;
using ShoppingList.Frontend.Redux.ShoppingList.States;

namespace ShoppingList.Frontend.Redux.ShoppingList.Reducers;

public static class ShoppingListSearchBarReducer
{
    [ReducerMethod]
    public static ShoppingListState OnItemForShoppingListSearchInputChanged(ShoppingListState state,
        ItemForShoppingListSearchInputChangedAction action)
    {
        var input = action.Input.Trim();
        return state with
        {
            SearchBar = state.SearchBar with
            {
                Input = input,
                Results = input == string.Empty
                    ? new List<SearchItemForShoppingListResult>()
                    : state.SearchBar.Results
            },
            TemporaryItemCreator = state.TemporaryItemCreator with
            {
                IsButtonEnabled = !string.IsNullOrEmpty(input)
            }
        };
    }

    [ReducerMethod]
    public static ShoppingListState OnSearchItemForShoppingListFinished(ShoppingListState state,
        SearchItemForShoppingListFinishedAction action)
    {
        return state with
        {
            SearchBar = state.SearchBar with
            {
                Results = action.Results.ToList()
            }
        };
    }

    [ReducerMethod(typeof(ItemForShoppingListSearchResultSelectedAction))]
    public static ShoppingListState OnItemForShoppingListSearchResultSelected(ShoppingListState state)
    {
        return state with
        {
            SearchBar = state.SearchBar with
            {
                Input = string.Empty,
                Results = new List<SearchItemForShoppingListResult>()
            },
            TemporaryItemCreator = state.TemporaryItemCreator with
            {
                ItemName = string.Empty
            }
        };
    }

    [ReducerMethod(typeof(SetSearchBarActiveAction))]
    public static ShoppingListState OnSetSearchBarActive(ShoppingListState state)
    {
        return state with
        {
            SearchBar = state.SearchBar with
            {
                IsActive = true
            }
        };
    }

    [ReducerMethod(typeof(SetSearchBarInactiveAction))]
    public static ShoppingListState OnSetSearchBarInactive(ShoppingListState state)
    {
        return state with
        {
            SearchBar = state.SearchBar with
            {
                IsActive = false
            }
        };
    }
}