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
    private readonly IState<RecipeState> _state;

    public RecipeEffects(IApiClient client, NavigationManager navigationManager, IState<RecipeState> state)
    {
        _client = client;
        _navigationManager = navigationManager;
        _state = state;
    }

    [EffectMethod(typeof(SearchRecipeByNameAction))]
    public async Task HandleSearchRecipeByNameAction(IDispatcher dispatcher)
    {
        if (string.IsNullOrWhiteSpace(_state.Value.Search.Input))
        {
            dispatcher.Dispatch(new SearchRecipeFinishedAction(new List<RecipeSearchResult>(0), SearchType.Name));
            return;
        }

        IEnumerable<RecipeSearchResult> results;
        try
        {
            results = await _client.SearchRecipesByNameAsync(_state.Value.Search.Input);
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

        dispatcher.Dispatch(new SearchRecipeFinishedAction(results.ToList(), SearchType.Name));
    }

    [EffectMethod(typeof(SearchRecipeByTagsAction))]
    public async Task HandleSearchRecipeByTagsAction(IDispatcher dispatcher)
    {
        if (!_state.Value.Search.SelectedRecipeTagIds.Any())
        {
            dispatcher.Dispatch(new SearchRecipeFinishedAction(new List<RecipeSearchResult>(0), SearchType.Tag));
            return;
        }

        IEnumerable<RecipeSearchResult> results;
        try
        {
            results = await _client.SearchRecipesByTagsAsync(_state.Value.Search.SelectedRecipeTagIds);
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

        dispatcher.Dispatch(new SearchRecipeFinishedAction(results.ToList(), SearchType.Tag));
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