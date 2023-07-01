using Fluxor;
using Microsoft.AspNetCore.Components;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.Actions.Editor;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.Actions.Editor.Availabilities;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Constants;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.Items;
using RestEase;
using Timer = System.Timers.Timer;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Items.Effects;

public sealed class ItemEditorEffects
{
    private const float LeaveEditorAfterDeleteDelayInSeconds = 0.5f;

    private readonly IApiClient _client;
    private readonly IState<ItemState> _state;
    private readonly NavigationManager _navigationManager;

    private Timer? _leaveEditorTimer;

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
        EditedItem item;
        try
        {
            item = await _client.GetItemByIdAsync(action.ItemId);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Loading item failed", e));
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Loading item failed", e.Message));
            return;
        }
        dispatcher.Dispatch(new LoadItemForEditingFinishedAction(item));
    }

    [EffectMethod]
    public static Task HandleAddStoreAction(AddStoreAction action, IDispatcher dispatcher)
    {
        if (action.Available is EditedItem)
            dispatcher.Dispatch(new StoreAddedToItemAction());
        if (action.Available is EditedItemType itemType)
            dispatcher.Dispatch(new StoreAddedToItemTypeAction(itemType.Id));

        return Task.CompletedTask;
    }

    [EffectMethod]
    public static Task HandleChangeStoreAction(ChangeStoreAction action, IDispatcher dispatcher)
    {
        if (action.Available is EditedItem)
            dispatcher.Dispatch(new StoreOfItemChangedAction(action.Availability, action.StoreId));
        if (action.Available is EditedItemType itemType)
            dispatcher.Dispatch(new StoreOfItemTypeChangedAction(itemType, action.Availability, action.StoreId));

        return Task.CompletedTask;
    }

    [EffectMethod]
    public static Task HandleChangePriceAction(ChangePriceAction action, IDispatcher dispatcher)
    {
        if (action.Available is EditedItem)
            dispatcher.Dispatch(new PriceOfItemChangedAction(action.Availability, action.Price));
        if (action.Available is EditedItemType itemType)
            dispatcher.Dispatch(new PriceOfItemTypeChangedAction(itemType, action.Availability, action.Price));

        return Task.CompletedTask;
    }

    [EffectMethod]
    public static Task HandleChangeDefaultSectionAction(ChangeDefaultSectionAction action, IDispatcher dispatcher)
    {
        if (action.Available is EditedItem)
            dispatcher.Dispatch(new DefaultSectionOfItemChangedAction(action.Availability, action.DefaultSectionId));
        if (action.Available is EditedItemType itemType)
            dispatcher.Dispatch(new DefaultSectionOfItemTypeChangedAction(itemType, action.Availability, action.DefaultSectionId));

        return Task.CompletedTask;
    }

    [EffectMethod]
    public static Task HandleRemoveStoreAction(RemoveStoreAction action, IDispatcher dispatcher)
    {
        if (action.Available is EditedItem)
            dispatcher.Dispatch(new StoreOfItemRemovedAction(action.Availability));
        if (action.Available is EditedItemType itemType)
            dispatcher.Dispatch(new StoreOfItemTypeRemovedAction(itemType, action.Availability));

        return Task.CompletedTask;
    }

    [EffectMethod(typeof(LeaveItemEditorAction))]
    public Task HandleLeaveItemEditorAction(IDispatcher dispatcher)
    {
        if (_leaveEditorTimer is not null)
        {
            _leaveEditorTimer.Stop();
            _leaveEditorTimer.Dispose();
        }

        _navigationManager.NavigateTo(PageRoutes.Items);
        return Task.CompletedTask;
    }

    [EffectMethod(typeof(CreateItemAction))]
    public async Task HandleCreateItemAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new CreateItemStartedAction());

        var item = _state.Value.Editor.Item!;
        try
        {
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
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Creating item failed", e));
            dispatcher.Dispatch(new CreateItemFinishedAction());
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Creating item failed", e.Message));
            dispatcher.Dispatch(new CreateItemFinishedAction());
            return;
        }

        dispatcher.Dispatch(new CreateItemFinishedAction());
        dispatcher.Dispatch(new LeaveItemEditorAction());
    }

    [EffectMethod(typeof(UpdateItemAction))]
    public async Task HandleUpdateItemAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new UpdateItemStartedAction());

        try
        {
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
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Updating item failed", e));
            dispatcher.Dispatch(new UpdateItemFinishedAction());
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Updating item failed", e.Message));
            dispatcher.Dispatch(new UpdateItemFinishedAction());
            return;
        }

        dispatcher.Dispatch(new UpdateItemFinishedAction());
        dispatcher.Dispatch(new LeaveItemEditorAction());
    }

    [EffectMethod(typeof(ModifyItemAction))]
    public async Task HandleModifyItemAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new ModifyItemStartedAction());

        try
        {
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
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Modifying item failed", e));
            dispatcher.Dispatch(new ModifyItemFinishedAction());
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Modifying item failed", e.Message));
            dispatcher.Dispatch(new ModifyItemFinishedAction());
            return;
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

        try
        {
            await _client.MakeTemporaryItemPermanent(request);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Saving item failed", e));
            dispatcher.Dispatch(new MakeItemPermanentFinishedAction());
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Saving item failed", e.Message));
            dispatcher.Dispatch(new MakeItemPermanentFinishedAction());
            return;
        }

        dispatcher.Dispatch(new MakeItemPermanentFinishedAction());
        dispatcher.Dispatch(new LeaveItemEditorAction());
    }

    [EffectMethod(typeof(DeleteItemAction))]
    public async Task HandleDeleteItemAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new DeleteItemStartedAction());

        var request = new DeleteItemRequest(Guid.NewGuid(), _state.Value.Editor.Item!.Id);

        try
        {
            await _client.DeleteItemAsync(request);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Deleting item failed", e));
            dispatcher.Dispatch(new DeleteItemFinishedAction());
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Deleting item failed", e.Message));
            dispatcher.Dispatch(new DeleteItemFinishedAction());
            return;
        }

        dispatcher.Dispatch(new DeleteItemFinishedAction());
        dispatcher.Dispatch(new CloseDeleteItemDialogAction(true));
    }

    [EffectMethod]
    public Task HandleCloseDeleteItemDialogAction(CloseDeleteItemDialogAction action, IDispatcher dispatcher)
    {
        if (!action.LeaveEditor)
            return Task.CompletedTask;

        if (_leaveEditorTimer is not null)
        {
            _leaveEditorTimer.Stop();
            _leaveEditorTimer.Dispose();
        }

        _leaveEditorTimer = new Timer(TimeSpan.FromSeconds(LeaveEditorAfterDeleteDelayInSeconds));
        _leaveEditorTimer.AutoReset = false;
        _leaveEditorTimer.Elapsed += (_, _) => dispatcher.Dispatch(new LeaveItemEditorAction());
        _leaveEditorTimer.Start();

        return Task.CompletedTask;
    }
}