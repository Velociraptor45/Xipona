using Fluxor;
using Microsoft.AspNetCore.Components;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Constants;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;
using RestEase;

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

        IEnumerable<RecipeSearchResult> results;
        try
        {
            results = await _client.SearchRecipesByNameAsync(action.SearchInput);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Searching for recipes failed", e));
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Searching for recipes failed", e.Message));
            return;
        }

        dispatcher.Dispatch(new SearchRecipeFinishedAction(results.ToList()));
    }

    [EffectMethod]
    public Task HandleEditRecipeAction(EditRecipeAction action, IDispatcher dispatcher)
    {
        _navigationManager.NavigateTo($"{PageRoutes.Recipes}/{action.Id}");
        return Task.CompletedTask;
    }

    [EffectMethod(typeof(LoadRecipeTagsAction))]
    public async Task HandleLoadRecipeTagsAction(IDispatcher dispatcher)
    {
        List<RecipeTag> recipeTags;
        try
        {
            recipeTags = (await _client.GetAllRecipeTagsAsync()).ToList();
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Loading recipe tags failed", e));
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Loading recipe tags failed", e.Message));
            return;
        }

        dispatcher.Dispatch(new LoadRecipeTagsFinishedAction(recipeTags));
    }
}