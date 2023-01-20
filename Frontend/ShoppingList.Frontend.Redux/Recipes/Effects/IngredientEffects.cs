using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor.Ingredients;
using ShoppingList.Frontend.Redux.Shared.Ports;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Effects;

public class IngredientEffects
{
    private readonly IApiClient _client;

    public IngredientEffects(IApiClient client)
    {
        _client = client;
    }

    [EffectMethod(typeof(LoadIngredientQuantityTypesAction))]
    public async Task HandleLoadIngredientQuantityTypesAction(IDispatcher dispatcher)
    {
        var results = await _client.GetAllIngredientQuantityTypes();
        dispatcher.Dispatch(new LoadIngredientQuantityTypesFinishedAction(results.ToList()));
    }

    [EffectMethod]
    public Task HandleLoadInitialItemsAction(LoadInitialItemsAction action, IDispatcher dispatcher)
    {
        if (action.Ingredient.ItemCategoryId != Guid.Empty)
            dispatcher.Dispatch(
                new LoadItemsForItemCategoryAction(action.Ingredient.Id, action.Ingredient.ItemCategoryId));

        return Task.CompletedTask;
    }
}