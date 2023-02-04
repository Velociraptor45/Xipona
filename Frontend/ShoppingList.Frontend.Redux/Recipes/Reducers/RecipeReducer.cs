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
                SearchResults = action.SearchResults
            }
        };
    }
}