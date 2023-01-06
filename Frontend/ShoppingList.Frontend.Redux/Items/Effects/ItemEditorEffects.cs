using Fluxor;
using ShoppingList.Frontend.Redux.Items.Actions.Editor;
using ShoppingList.Frontend.Redux.Items.Actions.Editor.Availabilities;
using ShoppingList.Frontend.Redux.Items.States;
using ShoppingList.Frontend.Redux.Manufacturers.States;
using ShoppingList.Frontend.Redux.Shared.Ports;

namespace ShoppingList.Frontend.Redux.Items.Effects;

public sealed class ItemEditorEffects
{
    private readonly IApiClient _client;
    private readonly IState<ItemState> _state;

    public ItemEditorEffects(IApiClient client, IState<ItemState> state)
    {
        _client = client;
        _state = state;
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

        var itemCategory = await _client.GetManufacturerByIdAsync(_state.Value.Editor.Item!.ManufacturerId!.Value);
        var result = new ManufacturerSearchResult(itemCategory.Id, itemCategory.Name);
        dispatcher.Dispatch(new LoadInitialManufacturerFinishedAction(result));
    }
}