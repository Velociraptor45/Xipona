using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor.AddToShoppingListModal;
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
            1,
            new List<EditedIngredient>(0),
            new SortedSet<EditedPreparationStep>(),
            new List<Guid>(0));

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = recipe,
                IsInEditMode = false
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
                Recipe = action.Recipe,
                IsInEditMode = false
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

    [ReducerMethod(typeof(ToggleEditModeAction))]
    public static RecipeState OnToggleEditMode(RecipeState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsInEditMode = !state.Editor.IsInEditMode
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnRecipeTagInputChanged(RecipeState state, RecipeTagInputChangedAction action)
    {
        return state with
        {
            Editor = state.Editor with
            {
                RecipeTagCreateInput = action.Input
            }
        };
    }

    [ReducerMethod(typeof(RecipeTagsDropdownClosedAction))]
    public static RecipeState OnRecipeTagsDropdownClosed(RecipeState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                RecipeTagCreateInput = string.Empty
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnRecipeTagsChanged(RecipeState state, RecipeTagsChangedAction action)
    {
        if (state.Editor.Recipe is null)
            return state;

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = state.Editor.Recipe with
                {
                    RecipeTagIds = action.RecipeTagIds
                }
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnCreateNewRecipeTagFinished(RecipeState state, CreateNewRecipeTagFinishedAction action)
    {
        if (state.Editor.Recipe is null)
            return state;

        var allTags = state.RecipeTags.ToList();
        allTags.Add(action.NewTag);

        var recipeTagIds = state.Editor.Recipe.RecipeTagIds.ToList();
        recipeTagIds.Add(action.NewTag.Id);

        return state with
        {
            RecipeTags = allTags.OrderBy(t => t.Name).ToList(),
            Editor = state.Editor with
            {
                Recipe = state.Editor.Recipe with
                {
                    RecipeTagIds = recipeTagIds
                }
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnRecipeNumberOfServingsChanged(RecipeState state,
        RecipeNumberOfServingsChangedAction action)
    {
        if (state.Editor.Recipe is null)
            return state;

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = state.Editor.Recipe with
                {
                    NumberOfServings = action.NumberOfServings
                }
            }
        };
    }

    [ReducerMethod(typeof(AddToShoppingListModalClosedAction))]
    public static RecipeState OnAddToShoppingListModalClosed(RecipeState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsAddToShoppingListOpen = false,
                AddToShoppingList = null
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnLoadAddToShoppingListFinished(RecipeState state,
        LoadAddToShoppingListFinishedAction action)
    {
        if (state.Editor.Recipe is null)
            return state;

        var ingredients = Multiply(action.Ingredients, state.Editor.Recipe.NumberOfServings).ToList();

        var addToShoppingList = new AddToShoppingList(
            state.Editor.Recipe.NumberOfServings,
            action.Ingredients,
            ingredients);

        return state with
        {
            Editor = state.Editor with
            {
                IsAddToShoppingListOpen = true,
                AddToShoppingList = addToShoppingList
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnAddToShoppingListNumberOfServingsChanged(RecipeState state,
        AddToShoppingListNumberOfServingsChangedAction action)
    {
        if (state.Editor.Recipe is null)
            return state;

        var ingredients = Multiply(state.Editor.AddToShoppingList!.IngredientsForOneServing, action.NumberOfServings)
            .ToList();

        return state with
        {
            Editor = state.Editor with
            {
                AddToShoppingList = state.Editor.AddToShoppingList with
                {
                    NumberOfServings = action.NumberOfServings,
                    Ingredients = ingredients
                }
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnAddIngredientToShoppingListChanged(RecipeState state,
        AddIngredientToShoppingListChangedAction action)
    {
        if (state.Editor.Recipe is null)
            return state;
        var ingredients = state.Editor.AddToShoppingList!.Ingredients.ToList();
        var ingredient = ingredients.FirstOrDefault(i => i.Key == action.IngredientKey);
        if (ingredient is null)
            return state;

        var ingredientIndex = ingredients.IndexOf(ingredient);
        ingredients[ingredientIndex] = ingredient with { AddToShoppingList = action.AddToShoppingList };

        return state with
        {
            Editor = state.Editor with
            {
                AddToShoppingList = state.Editor.AddToShoppingList with
                {
                    Ingredients = ingredients,
                }
            }
        };
    }

    private static IEnumerable<AddToShoppingListIngredient> Multiply(IEnumerable<AddToShoppingListIngredient> ingredients,
        int numberOfServings)
    {
        foreach (var ingredient in ingredients)
        {
            yield return ingredient with { Quantity = (float)Math.Ceiling(ingredient.Quantity * numberOfServings) };
        }
    }
}