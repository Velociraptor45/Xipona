using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Reducers;

public static class RecipeReducer
{
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
                SearchResults = action.SearchResults.OrderBy(r => r.Name).ToList()
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
}