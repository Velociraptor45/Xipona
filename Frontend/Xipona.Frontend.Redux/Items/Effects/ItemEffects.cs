﻿using Fluxor;
using Microsoft.AspNetCore.Components;
using ProjectHermes.Xipona.Frontend.Redux.Items.Actions;
using ProjectHermes.Xipona.Frontend.Redux.Items.Actions.Editor;
using ProjectHermes.Xipona.Frontend.Redux.Items.Actions.Search;
using ProjectHermes.Xipona.Frontend.Redux.Items.States;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Actions;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Constants;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports;
using ProjectHermes.Xipona.Frontend.Redux.Shared.States;
using RestEase;

namespace ProjectHermes.Xipona.Frontend.Redux.Items.Effects;

public class ItemEffects
{
    private readonly IApiClient _client;
    private readonly NavigationManager _navigationManager;
    private readonly IState<ItemState> _state;

    public ItemEffects(IApiClient client, NavigationManager navigationManager, IState<ItemState> state)
    {
        _client = client;
        _navigationManager = navigationManager;
        _state = state;
    }

    [EffectMethod]
    public Task HandleEditItemAction(EditItemAction action, IDispatcher dispatcher)
    {
        _navigationManager.NavigateTo($"{PageRoutes.Items}/{action.Id}");
        return Task.CompletedTask;
    }

    [EffectMethod(typeof(EnterItemSearchPageAction))]
    public static Task HandleEnterItemSearchPageAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new LoadQuantityTypesAction());
        dispatcher.Dispatch(new LoadQuantityTypesInPacketAction());
        dispatcher.Dispatch(new LoadActiveStoresAction());

        return Task.CompletedTask;
    }

    [EffectMethod(typeof(RetrieveSearchResultCountAction))]
    public async Task HandleRetrieveSearchResultCountAction(IDispatcher dispatcher)
    {
        if (string.IsNullOrWhiteSpace(_state.Value.Search.Input))
        {
            dispatcher.Dispatch(new RetrieveSearchResultCountFinishedAction(0));
            dispatcher.Dispatch(new SearchPageChangedAction(1));
            return;
        }

        dispatcher.Dispatch(new RetrieveSearchResultCountStartedAction());

        int count;
        try
        {
            count = await _client.GetTotalSearchResultCountAsync(_state.Value.Search.Input);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Retrieving search result count failed", e));
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Retrieving search result count failed", e.Message));
            return;
        }

        dispatcher.Dispatch(new RetrieveSearchResultCountFinishedAction(count));
        dispatcher.Dispatch(new SearchPageChangedAction(1));
    }

    [EffectMethod(typeof(SearchPageChangedAction))]
    public async Task HandleSearchPageChangedAction(IDispatcher dispatcher)
    {
        if (_state.Value.Search.TotalResultCount == 0)
        {
            dispatcher.Dispatch(new SearchItemsFinishedAction([]));
            return;
        }

        dispatcher.Dispatch(new SearchItemsStartedAction());

        IEnumerable<ItemSearchResult> result;
        try
        {
            result = await _client.SearchItemsAsync(_state.Value.Search.Input, _state.Value.Search.Page,
                _state.Value.Search.PageSize);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Searching for items failed", e));
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Searching for items failed", e.Message));
            return;
        }

        var finishAction = new SearchItemsFinishedAction(result.ToList());
        dispatcher.Dispatch(finishAction);
    }

    [EffectMethod(typeof(LoadQuantityTypesAction))]
    public async Task HandleLoadQuantityTypesAction(IDispatcher dispatcher)
    {
        if (_state.Value.QuantityTypes.Count != 0)
            return;

        IEnumerable<QuantityType> quantityTypes;
        try
        {
            quantityTypes = await _client.GetAllQuantityTypesAsync();
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Loading initial page state failed", e));
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Loading initial page state failed", e.Message));
            return;
        }

        dispatcher.Dispatch(new LoadQuantityTypesFinishedAction(quantityTypes.ToList()));
    }

    [EffectMethod(typeof(LoadQuantityTypesInPacketAction))]
    public async Task HandleLoadQuantityTypesInPacketAction(IDispatcher dispatcher)
    {
        if (_state.Value.QuantityTypesInPacket.Count != 0)
            return;

        IEnumerable<QuantityTypeInPacket> quantityTypes;
        try
        {
            quantityTypes = await _client.GetAllQuantityTypesInPacketAsync();
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Loading initial page state failed", e));
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Loading initial page state failed", e.Message));
            return;
        }
        dispatcher.Dispatch(new LoadQuantityTypesInPacketFinishedAction(quantityTypes.ToList()));
    }

    [EffectMethod(typeof(LoadActiveStoresAction))]
    public async Task HandleLoadActiveStoresAction(IDispatcher dispatcher)
    {
        if (_state.Value.Stores.Stores.Count != 0)
            return;

        IEnumerable<ItemStore> stores;
        try
        {
            stores = await _client.GetAllActiveStoresForItemAsync();
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Loading initial page state failed", e));
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Loading initial page state failed", e.Message));
            return;
        }

        dispatcher.Dispatch(new LoadActiveStoresFinishedAction(new ActiveStores(stores.ToList())));
    }
}