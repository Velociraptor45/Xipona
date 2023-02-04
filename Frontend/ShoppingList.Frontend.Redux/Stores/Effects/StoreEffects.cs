using Fluxor;
using Microsoft.AspNetCore.Components;
using ProjectHermes.ShoppingList.Frontend.Redux.Stores.Actions;
using ShoppingList.Frontend.Redux.Shared.Constants;
using ShoppingList.Frontend.Redux.Shared.Ports;

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
        var searchResults = await _client.GetActiveStoresOverviewAsync();

        dispatcher.Dispatch(new LoadStoresOverviewFinishedAction(searchResults.OrderBy(s => s.Name).ToList()));
    }

    [EffectMethod]
    public Task HandleSelectStoreAction(EditStoreAction action, IDispatcher dispatcher)
    {
        _navigationManager.NavigateTo($"{PageRoutes.Stores}/{action.Id}");
        return Task.CompletedTask;
    }
}