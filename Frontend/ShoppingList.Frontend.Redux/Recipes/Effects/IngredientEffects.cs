using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor.Ingredients;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;
using RestEase;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Effects;

public class IngredientEffects
{
    private readonly IApiClient _client;
    private readonly IState<RecipeState> _state;

    public IngredientEffects(IApiClient client, IState<RecipeState> state)
    {
        _client = client;
        _state = state;
    }

    [EffectMethod(typeof(LoadIngredientQuantityTypesAction))]
    public async Task HandleLoadIngredientQuantityTypesAction(IDispatcher dispatcher)
    {
        IEnumerable<IngredientQuantityType> results;
        try
        {
            results = await _client.GetAllIngredientQuantityTypes();
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Loading ingredient types failed", e));
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Loading ingredient types failed", e.Message));
            return;
        }

        dispatcher.Dispatch(new LoadIngredientQuantityTypesFinishedAction(results.ToList()));
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