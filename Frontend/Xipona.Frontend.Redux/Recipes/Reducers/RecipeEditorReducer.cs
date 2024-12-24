using Fluxor;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.Actions.Editor;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.States;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.States.Validators;
using ProjectHermes.Xipona.Frontend.Redux.Shared.States.Validators;

namespace ProjectHermes.Xipona.Frontend.Redux.Recipes.Reducers;

public static class RecipeEditorReducer
{
    private static readonly NameValidator _recipeNameValidator = new();
    private static readonly IngredientItemCategoryValidator _ingredientItemCategoryValidator = new();

    [ReducerMethod(typeof(CreateRecipeAction))]
    public static RecipeState OnCreateRecipe(RecipeState state)
    {
        return OnSaveRecipe(state);
    }

    [ReducerMethod(typeof(ModifyRecipeAction))]
    public static RecipeState OnModifyRecipe(RecipeState state)
    {
        return OnSaveRecipe(state);
    }

    private static RecipeState OnSaveRecipe(RecipeState state)
    {
        if (state.Editor.Recipe is null)
            return state;

        _recipeNameValidator.Validate(state.Editor.Recipe.Name, out var recipeNameError);

        var ingredientItemCategoryErrors = new Dictionary<Guid, string>();
        foreach (var ingredient in state.Editor.Recipe.Ingredients)
        {
            _ingredientItemCategoryValidator.Validate(ingredient.ItemCategoryId, out var error);
            if (error is not null)
                ingredientItemCategoryErrors.Add(ingredient.Key, error);
        }

        return state with
        {
            Editor = state.Editor with
            {
                ValidationResult = new(recipeNameError, ingredientItemCategoryErrors)
            }
        };
    }

    [ReducerMethod(typeof(SetNewRecipeAction))]
    public static RecipeState OnSetNewRecipe(RecipeState state)
    {
        var recipe = new EditedRecipe(
            Guid.Empty,
            string.Empty,
            1,
            new List<EditedIngredient>
            {
                EditedIngredient.GetInitial(state.IngredientQuantityTypes)
            },
            new SortedSet<EditedPreparationStep>
            {
                new(Guid.NewGuid(), Guid.Empty, string.Empty, 0)
            },
            new List<Guid>(0),
            null);

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = recipe,
                SideDishSelector = new SideDishSelector([], string.Empty),
                IsInEditMode = true,
                ValidationResult = new()
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
                SideDishSelector = new SideDishSelector(
                    action.Recipe.SideDish is null
                        ? []
                        : [action.Recipe.SideDish],
                    string.Empty),
                IsInEditMode = false,
                ValidationResult = new()
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnRecipeNameChanged(RecipeState state, RecipeNameChangedAction action)
    {
        if (state.Editor.Recipe is null)
            return state;

        _recipeNameValidator.Validate(action.Name, out var nameErrorMessage);

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = state.Editor.Recipe with
                {
                    Name = action.Name
                },
                ValidationResult = state.Editor.ValidationResult with
                {
                    Name = nameErrorMessage
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

    [ReducerMethod(typeof(ModifyRecipeStartedAction))]
    public static RecipeState OnModifyRecipeStarted(RecipeState state)
    {
        return SetSaving(state, true);
    }

    [ReducerMethod(typeof(ModifyRecipeFinishedAction))]
    public static RecipeState OnModifyRecipeFinished(RecipeState state)
    {
        return SetSaving(state, false);
    }

    [ReducerMethod(typeof(CreateRecipeStartedAction))]
    public static RecipeState OnCreateRecipeStarted(RecipeState state)
    {
        return SetSaving(state, true);
    }

    [ReducerMethod(typeof(CreateRecipeFinishedAction))]
    public static RecipeState OnCreateRecipeFinished(RecipeState state)
    {
        return SetSaving(state, false);
    }

    private static RecipeState SetSaving(RecipeState state, bool isSaving)
    {
        if (state.Editor.Recipe is null)
            return state;

        return state with
        {
            Editor = state.Editor with
            {
                IsSaving = isSaving
            }
        };
    }
}