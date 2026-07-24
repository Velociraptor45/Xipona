using Fluxor;
using RestEase;
using Xipona.Frontend.Redux.ItemCategories.States;
using Xipona.Frontend.Redux.Items.Actions.Filter;
using Xipona.Frontend.Redux.Items.States;
using Xipona.Frontend.Redux.Shared.Actions;
using Xipona.Frontend.Redux.Shared.Ports;
using Timer = System.Timers.Timer;

namespace Xipona.Frontend.Redux.Items.Effects;

public sealed class ItemFilterEffects(IApiClient client, IState<ItemState> state) : IAsyncDisposable
{
    private Timer? _startSearchTimer;
    
    [EffectMethod(typeof(LoadFilteredItemsAction))]
    public async Task HandleLoadFilteredItemsAction(IDispatcher dispatcher)
    {
        var filter = state.Value.Search.Filter;

        dispatcher.Dispatch(new LoadFilteredItemsStartedAction());
        
        var result = await client.FilterItemsAsync(filter.SelectedStore?.Id,
            filter.ItemCategoryFilter.SelectedItemCategory?.Id, null, 1, 20);
        
        dispatcher.Dispatch(new LoadFilteredItemsFinishedAction(result));
    }

    [EffectMethod]
    public Task HandleItemCategoryInputChangedAction(ItemCategoryInputChangedAction action, IDispatcher dispatcher)
    {
        if (_startSearchTimer is not null)
        {
            _startSearchTimer.Stop();
            _startSearchTimer.Dispose();
        }

        if (string.IsNullOrWhiteSpace(action.Input))
        {
            dispatcher.Dispatch(new SearchItemCategoriesFinishedAction([]));
            return Task.CompletedTask;
        }

        _startSearchTimer = new(300d);
        _startSearchTimer.AutoReset = false;
        _startSearchTimer.Elapsed += (_, _) => dispatcher.Dispatch(new SearchItemCategoriesAction());
        _startSearchTimer.Start();

        return Task.CompletedTask;
    }

    [EffectMethod(typeof(SearchItemCategoriesAction))]
    public async Task HandleSearchItemCategoriesAction(IDispatcher dispatcher)
    {
        var input = state.Value.Search.Filter.ItemCategoryFilter.Input;
        if (string.IsNullOrWhiteSpace(input))
            return;

        IEnumerable<ItemCategorySearchResult> results;
        try
        {
            results = await client.GetItemCategorySearchResultsAsync(input);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Searching for item categories failed", e));
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Searching for item categories failed", e.Message));
            return;
        }

        dispatcher.Dispatch(new SearchItemCategoriesFinishedAction([.. results]));
    }

    public ValueTask DisposeAsync()
    {
        _startSearchTimer?.Dispose();
        return ValueTask.CompletedTask;
    }
}