using Fluxor;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Actions;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports.Requests.Items;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;
using ProjectHermes.Xipona.Frontend.Redux.Shared.States;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.InitialStoreCreator;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.Persistence;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.PriceUpdater;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.Summary;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.TemporaryItemCreator;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States.Comparer;
using ProjectHermes.Xipona.Frontend.Redux.Stores.States;
using RestEase;
using ShoppingListItem = ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States.ShoppingListItem;

namespace ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Effects;

public class ShoppingListEffects
{
    private const int _quantityTypeUnitId = 0;
    private readonly IApiClient _client;
    private readonly ICommandQueue _commandQueue;
    private readonly IState<ShoppingListState> _state;
    private readonly IShoppingListNotificationService _notificationService;

    public ShoppingListEffects(IApiClient client, ICommandQueue commandQueue, IState<ShoppingListState> state,
        IShoppingListNotificationService notificationService)
    {
        _client = client;
        _commandQueue = commandQueue;
        _state = state;
        _notificationService = notificationService;
    }

    [EffectMethod(typeof(LoadQuantityTypesAction))]
    public async Task HandleLoadQuantityTypesAction(IDispatcher dispatcher)
    {
        if (_state.Value.QuantityTypes.Any())
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
        if (_state.Value.QuantityTypesInPacket.Any())
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

    [EffectMethod(typeof(LoadAllActiveStoresAction))]
    public async Task HandleLoadAllActiveStoresAction(IDispatcher dispatcher)
    {
        if (_state.Value.Stores.Stores.Any())
            return;

        List<ShoppingListStore> stores;
        try
        {
            stores = (await _client.GetAllActiveStoresForShoppingListAsync()).ToList();
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

        if (stores.Count == 0)
            dispatcher.Dispatch(new NoStoresFoundAction());

        var finishAction = new LoadAllActiveStoresFinishedAction(stores);
        dispatcher.Dispatch(finishAction);

        if (stores.Any())
            dispatcher.Dispatch(new SelectedStoreChangedAction(stores.OrderBy(s => s.Name).First().Id));
    }

    [EffectMethod]
    public async Task HandleSelectedStoreChangedAction(SelectedStoreChangedAction action, IDispatcher dispatcher)
    {
        var success = await LoadShoppingList(action.StoreId, dispatcher);
        if (success)
            dispatcher.Dispatch(new ResetEditModeAction());
    }

    [EffectMethod(typeof(ReloadCurrentShoppingListAction))]
    public async Task HandleReloadCurrentShoppingListAction(IDispatcher dispatcher)
    {
        await LoadShoppingList(_state.Value.SelectedStoreId, dispatcher);
    }

    [EffectMethod(typeof(SaveTemporaryItemAction))]
    public async Task HandleSaveTemporaryItemAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new SaveTemporaryItemStartedAction());

        var selectedQuantityTypeId = _state.Value.TemporaryItemCreator.SelectedQuantityTypeId;
        var quantityType = _state.Value.QuantityTypes.First(t => t.Id == selectedQuantityTypeId);
        var quantityTypeInPacket = selectedQuantityTypeId == _quantityTypeUnitId
            ? _state.Value.QuantityTypesInPacket.First()
            : null;
        var quantityInPacket = selectedQuantityTypeId == _quantityTypeUnitId ? 1f : (float?)null;

        var offlineId = ShoppingListItemId.FromOfflineId(Guid.NewGuid());
        var item = new ShoppingListItem(
            offlineId,
            TypeId: null,
            _state.Value.TemporaryItemCreator.ItemName,
            IsTemporary: true,
            _state.Value.TemporaryItemCreator.Price,
            quantityType,
            QuantityInPacket: quantityInPacket,
            quantityTypeInPacket,
            ItemCategory: "",
            Manufacturer: "",
            IsInBasket: false,
            Quantity: quantityType.DefaultQuantity,
            false);

        var addRequest = new AddTemporaryItemToShoppingListRequest(
            Guid.NewGuid(),
            _state.Value.ShoppingList!.Id,
            item.Name,
            item.QuantityType.Id,
            item.Quantity,
            item.PricePerQuantity,
            _state.Value.TemporaryItemCreator.Section!.Id,
            item.Id.OfflineId!.Value);

        dispatcher.Dispatch(new AddTemporaryItemAction(item, _state.Value.TemporaryItemCreator.Section));

        await _commandQueue.Enqueue(addRequest);

        dispatcher.Dispatch(new SaveTemporaryItemFinishedAction());
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
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Updating item price failed", e.Message));
            return;
        }

        dispatcher.Dispatch(new SavePriceUpdateFinishedAction());

        dispatcher.Dispatch(new ClosePriceUpdaterAction());
        dispatcher.Dispatch(new ReloadCurrentShoppingListAction());
        _notificationService.NotifySuccess("Successfully updated item price");
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
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Finishing shopping list failed", e.Message));
            dispatcher.Dispatch(new FinishShoppingListFinishedAction());
            return;
        }

        dispatcher.Dispatch(new FinishShoppingListFinishedAction());
        dispatcher.Dispatch(new ReloadCurrentShoppingListAction());
        dispatcher.Dispatch(new ResetEditModeAction());
        _notificationService.NotifySuccess("Finished shopping list");
    }

    private async Task<bool> LoadShoppingList(Guid storeId, IDispatcher dispatcher)
    {
        ShoppingListModel shoppingList;
        try
        {
            shoppingList = await _client.GetActiveShoppingListByStoreIdAsync(storeId);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Loading shopping list failed", e));
            return false;
        }
        catch (HttpRequestException)
        {
            dispatcher.Dispatch(new LoadShoppingListFromLocalStorageAction(storeId));
            return false;
        }

        dispatcher.Dispatch(new LoadShoppingListFinishedAction(shoppingList));
        return true;
    }

    [EffectMethod(typeof(CreateInitialStoreAction))]
    public async Task HandleCreateInitialStoreAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new CreateInitialStoreStartedAction());

        var store = new EditedStore(
            Guid.Empty,
            _state.Value.InitialStoreCreator.Name,
            new SortedSet<EditedSection>(new SortingIndexComparer())
            {
                new(Guid.Empty, Guid.Empty, "Default", true, 0)
            });

        try
        {
            await _client.CreateStoreAsync(store);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Creating initial store failed", e));
            dispatcher.Dispatch(new CreateInitialStoreFinishedAction());
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Creating initial store failed", e.Message));
            dispatcher.Dispatch(new CreateInitialStoreFinishedAction());
            return;
        }

        dispatcher.Dispatch(new CreateInitialStoreFinishedAction());
        dispatcher.Dispatch(new LoadAllActiveStoresAction());
    }
}