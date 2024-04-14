using Fluxor;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.Actions.Editor.Ingredients;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.States;

namespace ProjectHermes.Xipona.Frontend.Redux.Recipes.Reducers;

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
        ingredients.Insert(
            0,
            EditedIngredient.GetInitial(state.IngredientQuantityTypes));

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
        var ingredient = ingredients.FirstOrDefault(x => x.Key == action.IngredientKey);
        if (ingredient is null)
            return state;

        var ingredientIndex = ingredients.IndexOf(ingredient);
        ingredients[ingredientIndex] = ingredient with { Quantity = action.Quantity };

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
        var ingredient = ingredients.FirstOrDefault(x => x.Key == action.IngredientKey);
        if (ingredient is null)
            return state;

        var ingredientIndex = ingredients.IndexOf(ingredient);
        ingredients[ingredientIndex] = ingredient with { QuantityTypeId = action.QuantityTypeId };

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
        var ingredient = ingredients.FirstOrDefault(x => x.Key == action.IngredientKey);
        if (ingredient is null)
            return state;

        if (ingredient.DefaultItemId == action.ItemId && ingredient.DefaultItemTypeId == action.ItemTypeId)
            return state;

        var defaultStoreId = ingredient.ItemSelector.Items
            .First(i => i.ItemId == action.ItemId && i.ItemTypeId == action.ItemTypeId)
            .Availabilities
            .First()
            .StoreId;

        var ingredientIndex = ingredients.IndexOf(ingredient);

        ingredients[ingredientIndex] = ingredient with
        {
            DefaultItemId = action.ItemId,
            DefaultItemTypeId = action.ItemTypeId,
            DefaultStoreId = defaultStoreId,
            AddToShoppingListByDefault = true
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
    public static RecipeState OnSelectedItemCleared(RecipeState state, SelectedItemClearedAction action)
    {
        if (state.Editor.Recipe is null)
            return state;

        var ingredients = state.Editor.Recipe.Ingredients.ToList();
        var ingredient = ingredients.FirstOrDefault(x => x.Key == action.IngredientKey);
        if (ingredient is null)
            return state;

        var ingredientIndex = ingredients.IndexOf(ingredient);
        ingredients[ingredientIndex] = ingredient with
        {
            DefaultItemId = null,
            DefaultItemTypeId = null,
            DefaultStoreId = null,
            AddToShoppingListByDefault = null
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
        ingredients[ingredientIndex] = ingredient with
        {
            ItemSelector = ingredient.ItemSelector with
            {
                Items = action.Items.OrderBy(i => i.Name).ToList()
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
    public static RecipeState OnIngredientDefaultStoreChanged(RecipeState state,
        IngredientDefaultStoreChangedAction action)
    {
        if (state.Editor.Recipe is null)
            return state;

        var ingredients = state.Editor.Recipe.Ingredients.ToList();
        var ingredient = ingredients.FirstOrDefault(i => i.Key == action.IngredientKey);
        if (ingredient is null)
            return state;

        var ingredientIndex = ingredients.IndexOf(ingredient);
        ingredients[ingredientIndex] = ingredient with
        {
            DefaultStoreId = action.StoreId
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
    public static RecipeState OnIngredientAddToShoppingListByDefaultChanged(RecipeState state,
        IngredientAddToShoppingListByDefaultChangedAction action)
    {
        if (state.Editor.Recipe is null)
            return state;

        var ingredients = state.Editor.Recipe.Ingredients.ToList();
        var ingredient = ingredients.FirstOrDefault(i => i.Key == action.IngredientKey);
        if (ingredient is null)
            return state;

        var ingredientIndex = ingredients.IndexOf(ingredient);
        ingredients[ingredientIndex] = ingredient with
        {
            AddToShoppingListByDefault = action.AddToShoppingListByDefault
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