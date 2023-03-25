using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Configurations;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions.SearchBar;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States;
using RestEase;
using Timer = System.Timers.Timer;

namespace ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Effects;

public sealed class ShoppingListSearchBarEffects : IDisposable
{
    private readonly IApiClient _client;
    private readonly IState<ShoppingListState> _state;
    private readonly ShoppingListConfiguration _config;
    private Timer? _startSearchTimer;
    private CancellationTokenSource? _searchCancellationTokenSource;

    public ShoppingListSearchBarEffects(IApiClient client, IState<ShoppingListState> state,
        ShoppingListConfiguration config)
    {
        _client = client;
        _state = state;
        _config = config;
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

        _startSearchTimer = new(_config.SearchDelayAfterInput);
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
        IEnumerable<SearchItemForShoppingListResult> results;
        try
        {
            results = await _client.SearchItemsForShoppingListAsync(_state.Value.SearchBar.Input,
                    _state.Value.SelectedStoreId, _searchCancellationTokenSource.Token);
        }
        catch (OperationCanceledException)
        {
            return;
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

        dispatcher.Dispatch(new SearchItemForShoppingListFinishedAction(results.ToList()));
    }

    [EffectMethod]
    public async Task HandleItemForShoppingListSearchResultSelectedAction(
        ItemForShoppingListSearchResultSelectedAction action, IDispatcher dispatcher)
    {
        try
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
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Adding item to shopping list failed", e));
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Adding item to shopping list failed", e.Message));
            return;
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