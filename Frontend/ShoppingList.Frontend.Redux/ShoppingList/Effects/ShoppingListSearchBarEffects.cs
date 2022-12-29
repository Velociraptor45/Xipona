using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models;
using ShoppingList.Frontend.Redux.Shared.Ports;
using ShoppingList.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;
using ShoppingList.Frontend.Redux.ShoppingList.Actions;
using ShoppingList.Frontend.Redux.ShoppingList.Actions.SearchBar;
using ShoppingList.Frontend.Redux.ShoppingList.States;
using Timer = System.Timers.Timer;

namespace ShoppingList.Frontend.Redux.ShoppingList.Effects;

public sealed class ShoppingListSearchBarEffects : IDisposable
{
    private readonly IApiClient _client;
    private readonly IState<ShoppingListState> _state;

    private Timer? _startSearchTimer;
    private CancellationTokenSource? _searchCancellationTokenSource;

    public ShoppingListSearchBarEffects(IApiClient client, IState<ShoppingListState> state)
    {
        _client = client;
        _state = state;
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
        _searchCancellationTokenSource?.Dispose();
    }
}