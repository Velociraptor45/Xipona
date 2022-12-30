using Fluxor;
using ShoppingList.Frontend.Redux.ItemCategories.Actions;
using ShoppingList.Frontend.Redux.ItemCategories.States;

namespace ShoppingList.Frontend.Redux.ItemCategories.Reducers;

public static class ItemCategoryReducer
{
    [ReducerMethod(typeof(SearchItemCategoriesStartedAction))]
    public static ItemCategoryState OnSearchItemCategoriesStarted(ItemCategoryState state)
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
    public static ItemCategoryState OnSearchItemCategoriesFinished(ItemCategoryState state,
        SearchItemCategoriesFinishedAction action)
    {
        return state with
        {
            Search = state.Search with
            {
                IsLoadingSearchResults = false,
                SearchResults = action.SearchResults.ToList()
            }
        };
    }
}