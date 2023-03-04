using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.Items;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.States;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions.PriceUpdater;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions.Summary;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions.TemporaryItemCreator;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States;
using RestEase;
using ShoppingListItem = ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States.ShoppingListItem;

namespace ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Effects;

public class ShoppingListEffects
{
    private readonly IApiClient _client;
    private readonly ICommandQueue _commandQueue;
    private readonly IState<ShoppingListState> _state;

    public ShoppingListEffects(IApiClient client, ICommandQueue commandQueue, IState<ShoppingListState> state)
    {
        _client = client;
        _commandQueue = commandQueue;
        _state = state;
    }

    [EffectMethod(typeof(LoadQuantityTypesAction))]
    public async Task HandleLoadQuantityTypesAction(IDispatcher dispatcher)
    {
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
        dispatcher.Dispatch(new LoadQuantityTypesFinishedAction(quantityTypes));
    }

    [EffectMethod(typeof(LoadQuantityTypesInPacketAction))]
    public async Task HandleLoadQuantityTypesInPacketAction(IDispatcher dispatcher)
    {
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
        dispatcher.Dispatch(new LoadQuantityTypesInPacketFinishedAction(quantityTypes));
    }

    [EffectMethod(typeof(LoadAllActiveStoresAction))]
    public async Task HandleLoadAllActiveStoresAction(IDispatcher dispatcher)
    {
        IEnumerable<ShoppingListStore> stores;
        try
        {
            stores = await _client.GetAllActiveStoresForShoppingListAsync();
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Loading stores failed", e));
            return;
        }

        var finishAction = new LoadAllActiveStoresFinishedAction(new AllActiveStores(stores.ToList()));
        dispatcher.Dispatch(finishAction);

        if (stores.Any())
            dispatcher.Dispatch(new SelectedStoreChangedAction(stores.First().Id));
    }

    [EffectMethod]
    public async Task HandleSelectedStoreChangedAction(SelectedStoreChangedAction action, IDispatcher dispatcher)
    {
        ShoppingListModel shoppingList;
        try
        {
            shoppingList = await _client.GetActiveShoppingListByStoreIdAsync(action.StoreId);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Loading shopping list failed", e));
            return;
        }

        dispatcher.Dispatch(new LoadShoppingListFinishedAction(shoppingList));
    }

    [EffectMethod(typeof(SaveTemporaryItemAction))]
    public async Task HandleSaveTemporaryItemAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new SaveTemporaryItemStartedAction());

        // todo merge these requests (#333)
        var quantityType = new QuantityType(0, "", 1, "€", "x", 1);
        var quantityTypeInPacket = new QuantityTypeInPacket(0, "", "");
        var offlineId = ShoppingListItemId.FromOfflineId(Guid.NewGuid());
        var item = new ShoppingListItem(
            offlineId,
            TypeId: null,
            _state.Value.TemporaryItemCreator.ItemName,
            IsTemporary: true,
            _state.Value.TemporaryItemCreator.Price,
            quantityType,
            QuantityInPacket: 1,
            quantityTypeInPacket,
            ItemCategory: "",
            Manufacturer: "",
            IsInBasket: false,
            Quantity: 1,
            false);

        var createRequest = new CreateTemporaryItemRequest(
            Guid.NewGuid(),
            item.Id.OfflineId!.Value,
            item.Name,
            _state.Value.SelectedStoreId,
            item.PricePerQuantity,
            _state.Value.TemporaryItemCreator.Section!.Id);
        var addRequest = new AddItemToShoppingListRequest(
            Guid.NewGuid(),
            _state.Value.ShoppingList!.Id,
            item.Id,
            1,
            null);

        await _commandQueue.Enqueue(createRequest);
        await _commandQueue.Enqueue(addRequest);

        dispatcher.Dispatch(new SaveTemporaryItemFinishedAction(item, _state.Value.TemporaryItemCreator.Section));
        dispatcher.Dispatch(new CloseTemporaryItemCreatorAction());
    }

    [EffectMethod(typeof(SavePriceUpdateAction))]
    public async Task HandleSavePriceUpdateAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new SavePriceUpdateStartedAction());

        var typeId = _state.Value.PriceUpdate.UpdatePriceForAllTypes ? null : _state.Value.PriceUpdate.Item!.TypeId;

        var request = new UpdateItemPriceRequest(
            _state.Value.PriceUpdate.Item!.Id.ActualId!.Value,
            typeId,
            _state.Value.SelectedStoreId,
            _state.Value.PriceUpdate.Price);

        try
        {
            await _client.UpdateItemPriceAsync(request);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Updating item price failed", e));
            return;
        }

        dispatcher.Dispatch(new SavePriceUpdateFinishedAction(
            _state.Value.PriceUpdate.Item.Id.ActualId!.Value,
            typeId,
            _state.Value.PriceUpdate.Price));

        dispatcher.Dispatch(new ClosePriceUpdaterAction());
    }

    [EffectMethod(typeof(FinishShoppingListAction))]
    public async Task HandleFinishShoppingListAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new FinishShoppingListStartedAction());

        var finishedAt = new DateTimeOffset(_state.Value.Summary.FinishedAt.ToUniversalTime(),
            TimeSpan.Zero);

        var request = new FinishListRequest(
            Guid.NewGuid(),
            _state.Value.ShoppingList!.Id,
            finishedAt);

        try
        {
            await _client.FinishListAsync(request);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Finishing shopping list failed", e));
            dispatcher.Dispatch(new FinishShoppingListFinishedAction());
            return;
        }

        dispatcher.Dispatch(new FinishShoppingListFinishedAction());
        dispatcher.Dispatch(new SelectedStoreChangedAction(_state.Value.SelectedStoreId));
    }
}