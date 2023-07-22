using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor.Ingredients;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Effects;

public class IngredientEffects
{
    private readonly IState<RecipeState> _state;

    public IngredientEffects(IState<RecipeState> state)
    {
        _state = state;
    }

    [EffectMethod]
    public Task HandleLoadInitialItemsAction(LoadInitialItemsAction action, IDispatcher dispatcher)
    {
        if (action.Ingredient.ItemCategoryId == Guid.Empty)
            return Task.CompletedTask;

        var ingredient = _state.Value.GetIngredientByKey(action.Ingredient.Key);
        if (ingredient is null || ingredient.ItemSelector.Items.Any())
            return Task.CompletedTask;

        dispatcher.Dispatch(
            new LoadItemsForItemCategoryAction(action.Ingredient.Key, action.Ingredient.ItemCategoryId));

        return Task.CompletedTask;
    }
}