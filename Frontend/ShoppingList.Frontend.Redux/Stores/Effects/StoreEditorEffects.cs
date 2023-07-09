﻿using Fluxor;
using Microsoft.AspNetCore.Components;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Constants;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;
using ProjectHermes.ShoppingList.Frontend.Redux.Stores.Actions.Editor;
using ProjectHermes.ShoppingList.Frontend.Redux.Stores.States;
using RestEase;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Stores.Effects;

public class StoreEditorEffects
{
    private readonly IApiClient _client;
    private readonly IState<StoreState> _state;
    private readonly NavigationManager _navigationManager;

    public StoreEditorEffects(IApiClient client, IState<StoreState> state, NavigationManager navigationManager)
    {
        _client = client;
        _state = state;
        _navigationManager = navigationManager;
    }

    [EffectMethod(typeof(LeaveStoreEditorAction))]
    public Task HandleLeaveStoreEditorAction(IDispatcher dispatcher)
    {
        _navigationManager.NavigateTo(PageRoutes.Stores);
        return Task.CompletedTask;
    }

    [EffectMethod]
    public async Task HandleLoadStoreForEditingAction(LoadStoreForEditingAction action, IDispatcher dispatcher)
    {
        EditedStore result;
        try
        {
            result = await _client.GetStoreByIdAsync(action.StoreId);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Loading store failed", e));
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Loading store failed", e.Message));
            return;
        }

        dispatcher.Dispatch(new LoadStoreForEditingFinishedAction(result));
    }

    [EffectMethod(typeof(SaveStoreAction))]
    public async Task HandleSaveStoreAction(IDispatcher dispatcher)
    {
        var store = _state.Value.Editor.Store;
        if (store is null)
            return;

        dispatcher.Dispatch(new SaveStoreStartedAction());

        if (store.Id == Guid.Empty)
        {
            try
            {
                await _client.CreateStoreAsync(store);
            }
            catch (ApiException e)
            {
                dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Creating store failed", e));
                dispatcher.Dispatch(new SaveStoreFinishedAction());
                return;
            }
            catch (HttpRequestException e)
            {
                dispatcher.Dispatch(new DisplayErrorNotificationAction("Creating store failed", e.Message));
                dispatcher.Dispatch(new SaveStoreFinishedAction());
                return;
            }
        }
        else
        {
            try
            {
                await _client.ModifyStoreAsync(store);
            }
            catch (ApiException e)
            {
                dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Modifying store failed", e));
                dispatcher.Dispatch(new SaveStoreFinishedAction());
                return;
            }
            catch (HttpRequestException e)
            {
                dispatcher.Dispatch(new DisplayErrorNotificationAction("Modifying store failed", e.Message));
                dispatcher.Dispatch(new SaveStoreFinishedAction());
                return;
            }
        }

        dispatcher.Dispatch(new SaveStoreFinishedAction());
        dispatcher.Dispatch(new LeaveStoreEditorAction());
    }

    [EffectMethod(typeof(DeleteStoreConfirmedAction))]
    public async Task HandleDeleteStoreConfirmedAction(IDispatcher dispatcher)
    {
        var store = _state.Value.Editor.Store;
        if (store is null)
            return;

        dispatcher.Dispatch(new DeleteStoreStartedAction());

        try
        {
            await _client.DeleteStoreAsync(store.Id);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Deleting store failed", e));
            dispatcher.Dispatch(new DeleteStoreFinishedAction());
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Deleting store failed", e.Message));
            dispatcher.Dispatch(new DeleteStoreFinishedAction());
            return;
        }

        dispatcher.Dispatch(new DeleteStoreFinishedAction());
        dispatcher.Dispatch(new CloseDeleteStoreDialogAction());
        dispatcher.Dispatch(new LeaveStoreEditorAction());
    }
}