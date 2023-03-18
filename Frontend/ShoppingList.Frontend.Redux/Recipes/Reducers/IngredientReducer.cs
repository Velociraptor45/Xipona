using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor.Ingredients;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Reducers;

public static class IngredientReducer
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

    [ReducerMethod(typeof(IngredientAddedAction))]
    public static RecipeState OnIngredientAdded(RecipeState state)
    {
        if (state.Editor.Recipe is null)
            return state;

        var ingredients = state.Editor.Recipe.Ingredients.ToList();
        ingredients.Add(new EditedIngredient(
            Guid.NewGuid(),
            Guid.Empty,
            Guid.Empty,
            state.IngredientQuantityTypes.First().Id,
            1,
            null,
            null,
            new ItemCategorySelector(
                new List<ItemCategorySearchResult>(0),
                string.Empty),
            new ItemSelector(new List<SearchItemByItemCategoryResult>(0))));

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
    public static RecipeState OnIngredientRemoved(RecipeState state, IngredientRemovedAction action)
    {
        if (state.Editor.Recipe is null)
            return state;

        var ingredients = state.Editor.Recipe.Ingredients.ToList();
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
    public static RecipeState OnIngredientQuantityChanged(RecipeState state, IngredientQuantityChangedAction action)
    {
        if (state.Editor.Recipe is null)
            return state;

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
        if (state.Editor.Recipe is null)
            return state;

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
    public static RecipeState OnSelectedItemChanged(RecipeState state, SelectedItemChangedAction action)
    {
        if (state.Editor.Recipe is null)
            return state;

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
    public static RecipeState OnLoadItemsForItemCategoryFinished(RecipeState state,
        LoadItemsForItemCategoryFinishedAction action)
    {
        if (state.Editor.Recipe is null)
            return state;

        var ingredients = state.Editor.Recipe.Ingredients.ToList();
        var ingredient = ingredients.FirstOrDefault(i => i.Key == action.IngredientKey);
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
}