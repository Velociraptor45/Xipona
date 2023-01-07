using Fluxor;
using Microsoft.AspNetCore.Components;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions;
using ShoppingList.Frontend.Redux.Recipes.Actions;
using ShoppingList.Frontend.Redux.Recipes.States;
using ShoppingList.Frontend.Redux.Shared.Constants;
using ShoppingList.Frontend.Redux.Shared.Ports;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Effects;

public class RecipeEffects
{
    private readonly IApiClient _client;
    private readonly NavigationManager _navigationManager;

    public RecipeEffects(IApiClient client, NavigationManager navigationManager)
    {
        _client = client;
        _navigationManager = navigationManager;
    }

    [EffectMethod]
    public async Task HandleSearchRecipeAction(SearchRecipeAction action, IDispatcher dispatcher)
    {
        if (string.IsNullOrWhiteSpace(action.SearchInput))
        {
            dispatcher.Dispatch(new SearchRecipeFinishedAction(new List<RecipeSearchResult>(0)));
            return;
        }

        var results = await _client.SearchRecipesByNameAsync(action.SearchInput);

        dispatcher.Dispatch(new SearchRecipeFinishedAction(results.ToList()));
    }

    [EffectMethod]
    public Task HandleEditRecipeAction(EditRecipeAction action, IDispatcher dispatcher)
    {
        _navigationManager.NavigateTo($"{PageRoutes.Recipes}/{action.Id}");
        return Task.CompletedTask;
    }
}