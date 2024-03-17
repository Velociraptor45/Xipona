using Fluxor;
using Microsoft.AspNetCore.Components;
using ProjectHermes.Xipona.Frontend.Redux.ItemCategories.Actions;
using ProjectHermes.Xipona.Frontend.Redux.ItemCategories.States;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Actions;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Constants;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports;
using RestEase;

namespace ProjectHermes.Xipona.Frontend.Redux.ItemCategories.Effects;

public class ItemCategoryEffects
{
    private readonly IApiClient _client;
    private readonly NavigationManager _navigationManager;
    private readonly IState<ItemCategoryState> _state;

    public ItemCategoryEffects(IApiClient client, NavigationManager navigationManager, IState<ItemCategoryState> state)
    {
        _client = client;
        _navigationManager = navigationManager;
        _state = state;
    }

    [EffectMethod(typeof(SearchItemCategoriesAction))]
    public async Task HandleSearchItemCategoriesAction(IDispatcher dispatcher)
    {
        if (string.IsNullOrWhiteSpace(_state.Value.Search.Input))
        {
            dispatcher.Dispatch(new SearchItemCategoriesFinishedAction(new List<ItemCategorySearchResult>()));
            return;
        }

        dispatcher.Dispatch(new SearchItemCategoriesStartedAction());

        IEnumerable<ItemCategorySearchResult> result;

        try
        {
            result = await _client.GetItemCategorySearchResultsAsync(_state.Value.Search.Input);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Loading search results failed", e));
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Loading search results failed", e.Message));
            return;
        }

        var finishAction = new SearchItemCategoriesFinishedAction(result.ToList());
        dispatcher.Dispatch(finishAction);
    }

    [EffectMethod]
    public Task HandleEditItemCategoryAction(EditItemCategoryAction action, IDispatcher dispatcher)
    {
        _navigationManager.NavigateTo($"{PageRoutes.ItemCategories}/{action.Id}");
        return Task.CompletedTask;
    }
}