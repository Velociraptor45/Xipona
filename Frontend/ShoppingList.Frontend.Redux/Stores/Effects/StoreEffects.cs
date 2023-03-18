using Fluxor;
using Microsoft.AspNetCore.Components;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Constants;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;
using ProjectHermes.ShoppingList.Frontend.Redux.Stores.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.Stores.States;
using RestEase;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Stores.Effects;

public class StoreEffects
{
    private readonly IApiClient _client;
    private readonly NavigationManager _navigationManager;

    public StoreEffects(IApiClient client, NavigationManager navigationManager)
    {
        _client = client;
        _navigationManager = navigationManager;
    }

    [EffectMethod(typeof(LoadStoresOverviewAction))]
    public async Task HandleLoadStoresOverviewAction(IDispatcher dispatcher)
    {
        IEnumerable<StoreSearchResult> searchResults;
        try
        {
            searchResults = await _client.GetActiveStoresOverviewAsync();
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Loading stores failed", e));
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Loading stores failed", e.Message));
            return;
        }

        dispatcher.Dispatch(new LoadStoresOverviewFinishedAction(searchResults.OrderBy(s => s.Name).ToList()));
    }

    [EffectMethod]
    public Task HandleSelectStoreAction(EditStoreAction action, IDispatcher dispatcher)
    {
        _navigationManager.NavigateTo($"{PageRoutes.Stores}/{action.Id}");
        return Task.CompletedTask;
    }
}