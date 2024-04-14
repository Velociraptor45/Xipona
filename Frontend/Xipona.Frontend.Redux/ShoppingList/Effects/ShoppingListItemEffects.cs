﻿using Fluxor;
using Microsoft.AspNetCore.Components;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Configurations;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Constants;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.Items;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;
using Timer = System.Timers.Timer;

namespace ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Effects;

public sealed class ShoppingListItemEffects : IDisposable
{
    private readonly ICommandQueue _commandQueue;
    private readonly IState<ShoppingListState> _state;
    private readonly NavigationManager _navigationManager;
    private readonly ShoppingListConfiguration _config;

    private Timer? _hideItemsTimer;

    public ShoppingListItemEffects(ICommandQueue commandQueue, IState<ShoppingListState> state,
        NavigationManager navigationManager, ShoppingListConfiguration config)
    {
        _commandQueue = commandQueue;
        _state = state;
        _navigationManager = navigationManager;
        _config = config;
    }

    [EffectMethod]
    public async Task HandlePutItemInBasketAction(PutItemInBasketAction action, IDispatcher dispatcher)
    {
        var request = new PutItemInBasketRequest(Guid.NewGuid(), _state.Value.ShoppingList!.Id, action.ItemId,
            action.ItemTypeId, action.ItemName);
        await _commandQueue.Enqueue(request);
        RestartHideItemsTimer(dispatcher);
    }

    [EffectMethod]
    public async Task HandleRemoveItemFromBasketAction(RemoveItemFromBasketAction action, IDispatcher dispatcher)
    {
        var request = new RemoveItemFromBasketRequest(Guid.NewGuid(), _state.Value.ShoppingList!.Id,
            action.ItemId, action.ItemTypeId, action.ItemName);
        await _commandQueue.Enqueue(request);
        RestartHideItemsTimer(dispatcher);
    }

    [EffectMethod]
    public async Task HandleChangeItemQuantityAction(ChangeItemQuantityAction action, IDispatcher dispatcher)
    {
        var item = _state.Value.ShoppingList!.Sections
            .SelectMany(s => s.Items)
            .FirstOrDefault(i => i.Id.ActualId == action.ItemId.ActualId
                                    && i.Id.OfflineId == action.ItemId.OfflineId
                                    && i.TypeId == action.ItemTypeId);
        if (item is null)
            return;

        var newQuantity = action.Type switch
        {
            ChangeItemQuantityAction.ChangeType.Diff => item.Quantity + action.Quantity,
            ChangeItemQuantityAction.ChangeType.Absolute => action.Quantity,
            _ => throw new ArgumentOutOfRangeException($"Handling of change type {action.Type} not implemented")
        };
        if (newQuantity < 1)
            newQuantity = 1;

        var request = new ChangeItemQuantityOnShoppingListRequest(Guid.NewGuid(), _state.Value.ShoppingList!.Id,
            action.ItemId, action.ItemTypeId, newQuantity, action.ItemName);
        await _commandQueue.Enqueue(request);

        dispatcher.Dispatch(new ChangeItemQuantityFinishedAction(item.Id, item.TypeId, newQuantity));
    }

    [EffectMethod]
    public async Task HandleRemoveItemFromShoppingListAction(RemoveItemFromShoppingListAction action,
        IDispatcher dispatcher)
    {
        var request = new RemoveItemFromShoppingListRequest(Guid.NewGuid(), _state.Value.ShoppingList!.Id,
            action.ItemId, action.ItemTypeId, action.ItemName);
        await _commandQueue.Enqueue(request);
    }

    [EffectMethod]
    public Task HandleMakeItemPermanentAction(MakeItemPermanentAction action, IDispatcher dispatcher)
    {
        _navigationManager.NavigateTo($"{PageRoutes.Items}/{action.ItemId}");
        return Task.CompletedTask;
    }

    private void RestartHideItemsTimer(IDispatcher dispatcher)
    {
        if (_hideItemsTimer is not null)
        {
            _hideItemsTimer.Stop();
            _hideItemsTimer.Dispose();
        }

        _hideItemsTimer = new(_config.HideItemsDelay);
        _hideItemsTimer.AutoReset = false;
        _hideItemsTimer.Elapsed += (_, _) =>
        {
            dispatcher.Dispatch(new HideItemsInBasketAction());
        };
        _hideItemsTimer.Start();
    }

    public void Dispose()
    {
        _hideItemsTimer?.Dispose();
    }
}