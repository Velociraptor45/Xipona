using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Reducers;

public static class RecipeEditorReducer
{
    [ReducerMethod(typeof(SetNewRecipeAction))]
    public static RecipeState OnSetNewRecipe(RecipeState state)
    {
        var recipe = new EditedRecipe(
            Guid.Empty,
            string.Empty,
            new List<EditedIngredient>(0),
            new SortedSet<EditedPreparationStep>());

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = recipe
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnLoadRecipeForEditingFinished(RecipeState state, LoadRecipeForEditingFinishedAction action)
    {
        return state with
        {
            Editor = state.Editor with
            {
                Recipe = action.Recipe
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnRecipeNameChanged(RecipeState state, RecipeNameChangedAction action)
    {
        if (state.Editor.Recipe is null)
            return state;

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = state.Editor.Recipe with
                {
                    Name = action.Name
                }
            }
        };
    }
}