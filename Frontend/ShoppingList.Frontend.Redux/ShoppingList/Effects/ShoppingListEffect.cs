using Fluxor;
using Microsoft.AspNetCore.Components;
using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Services;
using ShoppingList.Frontend.Redux.Shared.Ports;
using ShoppingList.Frontend.Redux.Shared.Ports.Requests.Items;
using ShoppingList.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;
using ShoppingList.Frontend.Redux.Shared.States;
using ShoppingList.Frontend.Redux.ShoppingList.Actions;
using ShoppingList.Frontend.Redux.ShoppingList.States;
using ShoppingListItem = ShoppingList.Frontend.Redux.ShoppingList.States.ShoppingListItem;
using Timer = System.Timers.Timer;

namespace ShoppingList.Frontend.Redux.ShoppingList.Effects;

public class ShoppingListEffects : IDisposable
{
    private const string ItemsPageName = "items";

    private readonly IApiClient _client;
    private readonly ICommandQueue _commandQueue;
    private readonly IState<ShoppingListState> _state;
    private readonly NavigationManager _navigationManager;
    private readonly ITemporaryItemCreationService _temporaryItemCreationService;

    private Timer _startSearchTimer;
    private CancellationTokenSource _searchCancellationTokenSource;

    public ShoppingListEffects(IApiClient client, ICommandQueue commandQueue, IState<ShoppingListState> state,
        NavigationManager navigationManager, ITemporaryItemCreationService temporaryItemCreationService)
    {
        _client = client;
        _commandQueue = commandQueue;
        _state = state;
        _navigationManager = navigationManager;
        _temporaryItemCreationService = temporaryItemCreationService;
    }

    [EffectMethod(typeof(LoadQuantityTypesAction))]
    public async Task HandleLoadQuantityTypesAction(IDispatcher dispatcher)
    {
        var quantityTypes = await _client.GetAllQuantityTypesAsync();
        dispatcher.Dispatch(new LoadQuantityTypesFinishedAction(quantityTypes));
    }

    [EffectMethod(typeof(LoadQuantityTypesInPacketAction))]
    public async Task HandleLoadQuantityTypesInPacketAction(IDispatcher dispatcher)
    {
        var quantityTypes = await _client.GetAllQuantityTypesInPacketAsync();
        dispatcher.Dispatch(new LoadQuantityTypesInPacketFinishedAction(quantityTypes));
    }

    [EffectMethod(typeof(LoadAllActiveStoresAction))]
    public async Task HandleLoadAllActiveStoresAction(IDispatcher dispatcher)
    {
        var stores = await _client.GetAllActiveStoresForShoppingListAsync();
        var finishAction = new LoadAllActiveStoresFinishedAction(new AllActiveStores(stores.ToList()));
        dispatcher.Dispatch(finishAction);

        if (stores.Any())
            dispatcher.Dispatch(new SelectedStoreChangedAction(stores.First().Id));
    }

    [EffectMethod]
    public async Task HandleSelectedStoreChangedAction(SelectedStoreChangedAction action, IDispatcher dispatcher)
    {
        var shoppingList = await _client.GetActiveShoppingListByStoreIdAsyncNew(action.StoreId);

        dispatcher.Dispatch(new LoadShoppingListFinishedAction(shoppingList));
    }

    [EffectMethod]
    public async Task HandlePutItemInBasketAction(PutItemInBasketAction action, IDispatcher dispatcher)
    {
        var request = new PutItemInBasketRequest(Guid.NewGuid(), _state.Value.ShoppingList!.Id, action.ItemId,
            action.ItemTypeId);
        await _commandQueue.Enqueue(request);
    }

    [EffectMethod]
    public async Task HandleRemoveItemFromBasketAction(RemoveItemFromBasketAction action, IDispatcher dispatcher)
    {
        var request = new RemoveItemFromBasketRequest(Guid.NewGuid(), _state.Value.ShoppingList!.Id,
            action.ItemId, action.ItemTypeId);
        await _commandQueue.Enqueue(request);
    }

    [EffectMethod]
    public async Task HandleChangeItemQuantityAction(ChangeItemQuantityAction action, IDispatcher dispatcher)
    {
        var request = new ChangeItemQuantityOnShoppingListRequest(Guid.NewGuid(), _state.Value.ShoppingList!.Id,
            action.ItemId, action.ItemTypeId, action.Quantity);
        await _commandQueue.Enqueue(request);
    }

    [EffectMethod]
    public async Task HandleRemoveItemFromShoppingListAction(RemoveItemFromShoppingListAction action,
        IDispatcher dispatcher)
    {
        var request = new RemoveItemFromShoppingListRequest(Guid.NewGuid(), _state.Value.ShoppingList!.Id,
            action.ItemId, action.ItemTypeId);
        await _commandQueue.Enqueue(request);
    }

    [EffectMethod]
    public Task HandleMakeItemPermanentAction(MakeItemPermanentAction action, IDispatcher dispatcher)
    {
        _navigationManager.NavigateTo($"{ItemsPageName}/{action.ItemId}");
        return Task.CompletedTask;
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

    [EffectMethod]
    public Task HandleItemForShoppingListSearchInputChangedAction(ItemForShoppingListSearchInputChangedAction action,
        IDispatcher dispatcher)
    {
        if (string.IsNullOrWhiteSpace(action.Input))
        {
            _startSearchTimer?.Stop();
            return Task.CompletedTask;
        }

        if (_startSearchTimer is not null)
        {
            _startSearchTimer.Stop();
            _startSearchTimer.Dispose();
        }

        _startSearchTimer = new(300d);
        _startSearchTimer.AutoReset = false;
        _startSearchTimer.Elapsed += (_, _) => dispatcher.Dispatch(new SearchItemForShoppingListAction());
        _startSearchTimer.Start();

        return Task.CompletedTask;
    }

    [EffectMethod]
    public async Task HandleSearchItemForShoppingListAction(SearchItemForShoppingListAction action,
        IDispatcher dispatcher)
    {
        if (string.IsNullOrWhiteSpace(_state.Value.SearchBar.Input))
        {
            return;
        }

        _searchCancellationTokenSource = CreateNewCancellationTokenSource();
        var results = await _client.SearchItemsForShoppingListAsync(_state.Value.SearchBar.Input,
            _state.Value.SelectedStoreId, _searchCancellationTokenSource.Token);
        dispatcher.Dispatch(new SearchItemForShoppingListFinishedAction(results));
    }

    [EffectMethod]
    public async Task HandleItemForShoppingListSearchResultSelectedAction(
        ItemForShoppingListSearchResultSelectedAction action, IDispatcher dispatcher)
    {
        if (action.Result.ItemTypeId is null)
        {
            var request = new AddItemToShoppingListRequest(
                Guid.NewGuid(),
                _state.Value.ShoppingList!.Id,
                ShoppingListItemId.FromActualId(action.Result.ItemId),
                action.Result.DefaultQuantity,
                action.Result.DefaultSectionId);
            await _client.AddItemToShoppingListAsync(request);
        }
        else
        {
            var request = new AddItemWithTypeToShoppingListRequest(
                Guid.NewGuid(),
                _state.Value.ShoppingList!.Id,
                action.Result.ItemId,
                action.Result.ItemTypeId.Value,
                action.Result.DefaultQuantity,
                action.Result.DefaultSectionId);
            await _client.AddItemWithTypeToShoppingListAsync(request);
        }

        dispatcher.Dispatch(new SelectedStoreChangedAction(_state.Value.SelectedStoreId));
    }

    private CancellationTokenSource CreateNewCancellationTokenSource()
    {
        if (_searchCancellationTokenSource is not null)
        {
            _searchCancellationTokenSource.Cancel();
            _searchCancellationTokenSource.Dispose();
        }
        return new CancellationTokenSource();
    }

    public void Dispose()
    {
        if (_searchCancellationTokenSource is not null)
            _searchCancellationTokenSource.Dispose();
    }
}