using Fluxor;
using Microsoft.AspNetCore.Components;
using ShoppingList.Frontend.Redux.Items.Actions.Editor;
using ShoppingList.Frontend.Redux.Items.Actions.Editor.Availabilities;
using ShoppingList.Frontend.Redux.Items.Actions.Editor.ManufacturerSelectors;
using ShoppingList.Frontend.Redux.Items.States;
using ShoppingList.Frontend.Redux.Manufacturers.States;
using ShoppingList.Frontend.Redux.Shared.Constants;
using ShoppingList.Frontend.Redux.Shared.Ports;
using ShoppingList.Frontend.Redux.Shared.Ports.Requests.Items;

namespace ShoppingList.Frontend.Redux.Items.Effects;

public sealed class ItemEditorEffects
{
    private readonly IApiClient _client;
    private readonly IState<ItemState> _state;
    private readonly NavigationManager _navigationManager;

    public ItemEditorEffects(IApiClient client, IState<ItemState> state, NavigationManager navigationManager)
    {
        _client = client;
        _state = state;
        _navigationManager = navigationManager;
    }

    [EffectMethod]
    public async Task HandleLoadItemForEditingAction(LoadItemForEditingAction action, IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new LoadItemForEditingStartedAction());
        var item = await _client.GetItemByIdAsync(action.ItemId);
        dispatcher.Dispatch(new LoadItemForEditingFinishedAction(item));
    }

    [EffectMethod]
    public Task HandleAddStoreAction(AddStoreAction action, IDispatcher dispatcher)
    {
        if (action.Available is EditedItem)
            dispatcher.Dispatch(new StoreAddedToItemAction());
        if (action.Available is EditedItemType itemType)
            dispatcher.Dispatch(new StoreAddedToItemTypeAction(itemType));

        return Task.CompletedTask;
    }

    [EffectMethod]
    public Task HandleChangeStoreAction(ChangeStoreAction action, IDispatcher dispatcher)
    {
        if (action.Available is EditedItem)
            dispatcher.Dispatch(new StoreOfItemChangedAction(action.Availability, action.StoreId));
        if (action.Available is EditedItemType itemType)
            dispatcher.Dispatch(new StoreOfItemTypeChangedAction(itemType, action.Availability, action.StoreId));

        return Task.CompletedTask;
    }

    [EffectMethod]
    public Task HandleChangePriceAction(ChangePriceAction action, IDispatcher dispatcher)
    {
        if (action.Available is EditedItem)
            dispatcher.Dispatch(new PriceOfItemChangedAction(action.Availability, action.Price));
        if (action.Available is EditedItemType itemType)
            dispatcher.Dispatch(new PriceOfItemTypeChangedAction(itemType, action.Availability, action.Price));

        return Task.CompletedTask;
    }

    [EffectMethod]
    public Task HandleChangeDefaultSectionAction(ChangeDefaultSectionAction action, IDispatcher dispatcher)
    {
        if (action.Available is EditedItem)
            dispatcher.Dispatch(new DefaultSectionOfItemChangedAction(action.Availability, action.DefaultSectionId));
        if (action.Available is EditedItemType itemType)
            dispatcher.Dispatch(new DefaultSectionOfItemTypeChangedAction(itemType, action.Availability, action.DefaultSectionId));

        return Task.CompletedTask;
    }

    [EffectMethod]
    public Task HandleRemoveStoreAction(RemoveStoreAction action, IDispatcher dispatcher)
    {
        if (action.Available is EditedItem)
            dispatcher.Dispatch(new StoreOfItemRemovedAction(action.Availability));
        if (action.Available is EditedItemType itemType)
            dispatcher.Dispatch(new StoreOfItemTypeRemovedAction(itemType, action.Availability));

        return Task.CompletedTask;
    }

    [EffectMethod(typeof(LoadInitialManufacturerAction))]
    public async Task HandleLoadInitialManufacturerAction(IDispatcher dispatcher)
    {
        if (_state.Value.Editor.Item?.ManufacturerId is null)
            return;

        var manufacturer = await _client.GetManufacturerByIdAsync(_state.Value.Editor.Item!.ManufacturerId!.Value);
        var result = new ManufacturerSearchResult(manufacturer.Id, manufacturer.Name);
        dispatcher.Dispatch(new LoadInitialManufacturerFinishedAction(result));
    }

    [EffectMethod(typeof(LeaveItemEditorAction))]
    public Task HandleLeaveItemEditorAction(IDispatcher dispatcher)
    {
        _navigationManager.NavigateTo(PageRoutes.Items);
        return Task.CompletedTask;
    }

    [EffectMethod(typeof(CreateItemAction))]
    public async Task HandleCreateItemAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new CreateItemStartedAction());

        var item = _state.Value.Editor.Item!;
        if (item.IsItemWithTypes || (item.ItemMode is ItemMode.NotDefined && item.ItemTypes.Count > 0))
        {
            var request = new CreateItemWithTypesRequest(Guid.NewGuid(), _state.Value.Editor.Item!);
            await _client.CreateItemWithTypesAsync(request);
        }
        else
        {
            var request = new CreateItemRequest(Guid.NewGuid(), _state.Value.Editor.Item!);
            await _client.CreateItemAsync(request);
        }

        dispatcher.Dispatch(new CreateItemFinishedAction());
        dispatcher.Dispatch(new LeaveItemEditorAction());
    }

    [EffectMethod(typeof(UpdateItemAction))]
    public async Task HandleUpdateItemAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new UpdateItemStartedAction());

        if (_state.Value.Editor.Item!.IsItemWithTypes)
        {
            var request = new UpdateItemWithTypesRequest(Guid.NewGuid(), _state.Value.Editor.Item!);
            await _client.UpdateItemWithTypesAsync(request);
        }
        else
        {
            var request = new UpdateItemRequest(Guid.NewGuid(), _state.Value.Editor.Item!);
            await _client.UpdateItemAsync(request);
        }

        dispatcher.Dispatch(new UpdateItemFinishedAction());
        dispatcher.Dispatch(new LeaveItemEditorAction());
    }

    [EffectMethod(typeof(ModifyItemAction))]
    public async Task HandleModifyItemAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new ModifyItemStartedAction());

        if (_state.Value.Editor.Item!.IsItemWithTypes)
        {
            var request = new ModifyItemWithTypesRequest(Guid.NewGuid(), _state.Value.Editor.Item!);
            await _client.ModifyItemWithTypesAsync(request);
        }
        else
        {
            var request = new ModifyItemRequest(Guid.NewGuid(), _state.Value.Editor.Item!);
            await _client.ModifyItemAsync(request);
        }

        dispatcher.Dispatch(new ModifyItemFinishedAction());
        dispatcher.Dispatch(new LeaveItemEditorAction());
    }

    [EffectMethod(typeof(MakeItemPermanentAction))]
    public async Task HandleMakeItemPermanentAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new MakeItemPermanentStartedAction());

        var item = _state.Value.Editor.Item!;
        var request = new MakeTemporaryItemPermanentRequest(
            item.Id,
            item.Name,
            item.Comment,
            item.QuantityType.Id,
            item.QuantityInPacket,
            item.QuantityInPacketType?.Id,
            item.ItemCategoryId!.Value,
            item.ManufacturerId,
            item.Availabilities);
        await _client.MakeTemporaryItemPermanent(request);

        dispatcher.Dispatch(new MakeItemPermanentFinishedAction());
        dispatcher.Dispatch(new LeaveItemEditorAction());
    }

    [EffectMethod(typeof(DeleteItemAction))]
    public async Task HandleDeleteItemAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new DeleteItemStartedAction());

        var request = new DeleteItemRequest(Guid.NewGuid(), _state.Value.Editor.Item!.Id);
        await _client.DeleteItemAsync(request);

        dispatcher.Dispatch(new DeleteItemFinishedAction());
        dispatcher.Dispatch(new LeaveItemEditorAction());
    }
}