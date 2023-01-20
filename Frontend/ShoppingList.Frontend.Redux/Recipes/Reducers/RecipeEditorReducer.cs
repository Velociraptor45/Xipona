using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
using ShoppingList.Frontend.Redux.ItemCategories.States;
using ShoppingList.Frontend.Redux.Recipes.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Reducers;

public static class RecipeEditorReducer
{
    [ReducerMethod]
    public static RecipeState OnLoadIngredientQuantityTypesFinished(RecipeState state,
        LoadIngredientQuantityTypesFinishedAction action)
    {
        return state with
        {
            IngredientQuantityTypes = action.QuantityTypes
        };
    }

    [ReducerMethod(typeof(SetNewRecipeAction))]
    public static RecipeState OnSetNewRecipe(RecipeState state)
    {
        var recipe = new EditedRecipe(
            Guid.Empty,
            string.Empty,
            new List<EditedIngredient>(0),
            new List<EditedPreparationStep>(0));

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
    public static RecipeState OnIncrementPreparationStep(RecipeState state, IncrementPreparationStepAction action)
    {
        var steps = state.Editor.Recipe!.PreparationSteps.ToList();

        int sectionIndex = steps.IndexOf(action.PreparationStep);
        if (sectionIndex == -1 || sectionIndex >= steps.Count - 1)
            return state;

        var tmp = steps[sectionIndex + 1];
        steps[sectionIndex + 1] = action.PreparationStep;
        steps[sectionIndex] = tmp;

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = state.Editor.Recipe with
                {
                    PreparationSteps = steps
                }
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnDecrementPreparationStep(RecipeState state, DecrementPreparationStepAction action)
    {
        var steps = state.Editor.Recipe!.PreparationSteps.ToList();

        int sectionIndex = steps.IndexOf(action.PreparationStep);
        if (sectionIndex is -1 or <= 0)
            return state;

        var tmp = steps[sectionIndex - 1];
        steps[sectionIndex - 1] = action.PreparationStep;
        steps[sectionIndex] = tmp;

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = state.Editor.Recipe with
                {
                    PreparationSteps = steps
                }
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnPreparationStepRemoved(RecipeState state, PreparationStepRemovedAction action)
    {
        var steps = state.Editor.Recipe!.PreparationSteps.ToList();
        steps.Remove(action.PreparationStep);

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = state.Editor.Recipe with
                {
                    PreparationSteps = steps
                }
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnIngredientRemoved(RecipeState state, IngredientRemovedAction action)
    {
        var ingredients = state.Editor.Recipe!.Ingredients.ToList();
        ingredients.Remove(action.Ingredient);

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = state.Editor.Recipe with
                {
                    Ingredients = ingredients
                }
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnItemCategoryInputChanged(RecipeState state, ItemCategoryInputChangedAction action)
    {
        var ingredients = state.Editor.Recipe.Ingredients.ToList();
        var ingredientIndex = ingredients.IndexOf(action.Ingredient);
        if (ingredientIndex < 0)
            return state;

        ingredients[ingredientIndex] = action.Ingredient with
        {
            ItemCategorySelector = action.Ingredient.ItemCategorySelector with
            {
                Input = action.Input
            }
        };

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = state.Editor.Recipe with
                {
                    Ingredients = ingredients
                }
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnIngredientQuantityChanged(RecipeState state, IngredientQuantityChangedAction action)
    {
        var ingredients = state.Editor.Recipe.Ingredients.ToList();
        var ingredientIndex = ingredients.IndexOf(action.Ingredient);
        if (ingredientIndex < 0)
            return state;

        ingredients[ingredientIndex] = action.Ingredient with { Quantity = action.Quantity };

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = state.Editor.Recipe with
                {
                    Ingredients = ingredients
                }
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnIngredientQuantityTypeChanged(RecipeState state,
        IngredientQuantityTypeChangedAction action)
    {
        var ingredients = state.Editor.Recipe.Ingredients.ToList();
        var ingredientIndex = ingredients.IndexOf(action.Ingredient);
        if (ingredientIndex < 0)
            return state;

        ingredients[ingredientIndex] = action.Ingredient with { QuantityTypeId = action.QuantityType.Id };

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = state.Editor.Recipe with
                {
                    Ingredients = ingredients
                }
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnItemCategoryDropdownClosed(RecipeState state, ItemCategoryDropdownClosedAction action)
    {
        var ingredients = state.Editor.Recipe.Ingredients.ToList();
        var ingredientIndex = ingredients.IndexOf(action.Ingredient);
        if (ingredientIndex < 0)
            return state;

        ingredients[ingredientIndex] = action.Ingredient with
        {
            ItemCategorySelector = action.Ingredient.ItemCategorySelector with
            {
                Input = string.Empty
            }
        };

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = state.Editor.Recipe with
                {
                    Ingredients = ingredients
                }
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnSelectedItemChanged(RecipeState state, SelectedItemChangedAction action)
    {
        var ingredients = state.Editor.Recipe.Ingredients.ToList();
        var ingredientIndex = ingredients.IndexOf(action.Ingredient);
        if (ingredientIndex < 0)
            return state;

        ingredients[ingredientIndex] = action.Ingredient with
        {
            DefaultItemId = action.Item.ItemId,
            DefaultItemTypeId = action.Item.ItemTypeId
        };

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = state.Editor.Recipe with
                {
                    Ingredients = ingredients
                }
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnSelectedItemCategoryChanged(RecipeState state, SelectedItemCategoryChangedAction action)
    {
        var ingredients = state.Editor.Recipe.Ingredients.ToList();
        var ingredient = ingredients.FirstOrDefault(i => i.Id == action.IngredientId);
        if (ingredient is null)
            return state;
        var ingredientIndex = ingredients.IndexOf(ingredient);
        if (ingredientIndex < 0)
            return state;

        ingredients[ingredientIndex] = ingredient with
        {
            ItemCategoryId = action.ItemCategoryId,
            ItemCategorySelector = ingredient.ItemCategorySelector with
            {
                ItemCategories = ingredient.ItemCategorySelector.ItemCategories
                    .Where(i => i.Id == action.ItemCategoryId)
                    .ToList(),
                Input = string.Empty
            }
        };

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = state.Editor.Recipe with
                {
                    Ingredients = ingredients
                }
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnLoadInitialItemCategoryFinished(RecipeState state,
        LoadInitialItemCategoryFinishedAction action)
    {
        var ingredients = state.Editor.Recipe.Ingredients.ToList();
        var ingredient = ingredients.FirstOrDefault(i => i.Id == action.IngredientId);
        if (ingredient is null)
            return state;

        var ingredientIndex = ingredients.IndexOf(ingredient);
        if (ingredientIndex < 0)
            return state;

        ingredients[ingredientIndex] = ingredient with
        {
            ItemCategorySelector = ingredient.ItemCategorySelector with
            {
                ItemCategories = new List<ItemCategorySearchResult> { action.Result }
            }
        };

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = state.Editor.Recipe with
                {
                    Ingredients = ingredients
                }
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnLoadItemsForItemCategoryFinished(RecipeState state,
        LoadItemsForItemCategoryFinishedAction action)
    {
        var ingredients = state.Editor.Recipe.Ingredients.ToList();
        var ingredient = ingredients.FirstOrDefault(i => i.Id == action.IngredientId);
        if (ingredient is null)
            return state;

        var ingredientIndex = ingredients.IndexOf(ingredient);
        if (ingredientIndex < 0)
            return state;

        ingredients[ingredientIndex] = ingredient with
        {
            ItemSelector = ingredient.ItemSelector with
            {
                Items = action.Items
            }
        };

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = state.Editor.Recipe with
                {
                    Ingredients = ingredients
                }
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnSearchItemCategoriesFinished(RecipeState state, SearchItemCategoriesFinishedAction action)
    {
        var ingredients = state.Editor.Recipe.Ingredients.ToList();
        var ingredient = ingredients.FirstOrDefault(i => i.Id == action.IngredientId);
        if (ingredient is null)
            return state;

        var ingredientIndex = ingredients.IndexOf(ingredient);
        if (ingredientIndex < 0)
            return state;

        var results = action.ItemCategories
            .OrderBy(r => r.Name)
            .ToList();

        var currentlySelected = ingredient.ItemCategorySelector.ItemCategories
            .FirstOrDefault(cat => cat.Id == ingredient.ItemCategoryId);

        if (currentlySelected != null && results.All(r => r.Id != currentlySelected.Id))
            results.Insert(0, currentlySelected);

        ingredients[ingredientIndex] = ingredient with
        {
            ItemCategorySelector = ingredient.ItemCategorySelector with
            {
                ItemCategories = results
            }
        };

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = state.Editor.Recipe with
                {
                    Ingredients = ingredients
                }
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnCreateNewItemCategoryFinished(RecipeState state,
        CreateNewItemCategoryFinishedAction action)
    {
        var ingredients = state.Editor.Recipe.Ingredients.ToList();
        var ingredient = ingredients.FirstOrDefault(i => i.Id == action.IngredientId);
        if (ingredient is null)
            return state;

        var ingredientIndex = ingredients.IndexOf(ingredient);
        if (ingredientIndex < 0)
            return state;

        ingredients[ingredientIndex] = ingredient with
        {
            ItemCategoryId = action.SearchResult.Id,
            ItemCategorySelector = ingredient.ItemCategorySelector with
            {
                ItemCategories = new List<ItemCategorySearchResult> { action.SearchResult },
                Input = string.Empty
            }
        };

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = state.Editor.Recipe with
                {
                    Ingredients = ingredients
                }
            }
        };
    }
}