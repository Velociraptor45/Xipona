using Fluxor;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.Actions;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.States;

namespace ProjectHermes.Xipona.Frontend.Redux.Recipes.Reducers;

public static class RecipeReducer
{
    [ReducerMethod]
    public static RecipeState OnRecipeSearchInputChanged(RecipeState state, RecipeSearchInputChangedAction action)
    {
        return state with
        {
            Search = state.Search with
            {
                Input = action.Input
            }
        };
    }

    [ReducerMethod(typeof(EnterRecipeSearchPageAction))]
    public static RecipeState OnEnterRecipeSearchPage(RecipeState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                Recipe = null
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnSearchRecipeFinished(RecipeState state, SearchRecipeFinishedAction action)
    {
        return state with
        {
            Search = state.Search with
            {
                TriggeredAtLeastOnce = true,
                SearchResults = action.SearchResults.OrderBy(r => r.Name).ToList(),
                LastSearchType = action.SearchType
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnLoadRecipeTagsFinished(RecipeState state, LoadRecipeTagsFinishedAction action)
    {
        return state with
        {
            RecipeTags = action.RecipeTags.OrderBy(t => t.Name).ToList()
        };
    }

    [ReducerMethod]
    public static RecipeState OnSelectedSearchRecipeTagIdsChanged(RecipeState state,
        SelectedSearchRecipeTagIdsChangedAction action)
    {
        return state with
        {
            Search = state.Search with
            {
                SelectedRecipeTagIds = action.SelectedRecipeTagIds
            }
        };
    }
}