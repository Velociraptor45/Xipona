﻿using Fluxor;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.SearchBar;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Reducers;

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

    [ReducerMethod(typeof(SearchItemForShoppingListAction))]
    public static ShoppingListState OnSearchItemForShoppingList(ShoppingListState state)
    {
        return state with
        {
            SearchBar = state.SearchBar with
            {
                Results = new List<SearchItemForShoppingListResult>()
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
                Results = action.Results.OrderBy(r => r.Name).ToList()
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
}