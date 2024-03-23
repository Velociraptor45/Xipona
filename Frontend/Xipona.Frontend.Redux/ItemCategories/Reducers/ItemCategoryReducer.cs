using Fluxor;
using ProjectHermes.Xipona.Frontend.Redux.ItemCategories.Actions;
using ProjectHermes.Xipona.Frontend.Redux.ItemCategories.States;

namespace ProjectHermes.Xipona.Frontend.Redux.ItemCategories.Reducers;

public static class ItemCategoryReducer
{
    [ReducerMethod]
    public static ItemCategoryState OnItemCategorySearchInputChanged(ItemCategoryState state,
        ItemCategorySearchInputChangedAction action)
    {
        return state with
        {
            Search = state.Search with
            {
                Input = action.Input
            }
        };
    }

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
                TriggeredAtLeastOnce = true,
                SearchResults = action.SearchResults.OrderBy(r => r.Name).ToList()
            }
        };
    }
}