using Fluxor;
using RestEase;
using Xipona.Frontend.Redux.ItemCategories.States;
using Xipona.Frontend.Redux.Items.Actions.Filter;
using Xipona.Frontend.Redux.Items.States;
using Xipona.Frontend.Redux.Manufacturers.States;
using Xipona.Frontend.Redux.Shared.Actions;
using Xipona.Frontend.Redux.Shared.Ports;
using Timer = System.Timers.Timer;

namespace Xipona.Frontend.Redux.Items.Effects;

public sealed class ItemFilterEffects(IApiClient client, IState<ItemState> state) : IAsyncDisposable
{
    private Timer? _itemCategoryStartSearchTimer;
    private Timer? _manufacturerStartSearchTimer;
    
    [EffectMethod(typeof(LoadFilteredItemsAction))]
    public async Task HandleLoadFilteredItemsAction(IDispatcher dispatcher)
    {
        var filter = state.Value.Search.Filter;

        dispatcher.Dispatch(new LoadFilteredItemsStartedAction());
        
        var result = await client.FilterItemsAsync(filter.SelectedStore?.Id,
            filter.ItemCategoryFilter.SelectedItemCategory?.Id, 
            filter.ManufacturerFilter.SelectedManufacturer?.Id,
            1, 20);
        
        dispatcher.Dispatch(new LoadFilteredItemsFinishedAction(result));
    }

    [EffectMethod]
    public Task HandleItemCategoryInputChangedAction(ItemCategoryInputChangedAction action, IDispatcher dispatcher)
    {
        if (_itemCategoryStartSearchTimer is not null)
        {
            _itemCategoryStartSearchTimer.Stop();
            _itemCategoryStartSearchTimer.Dispose();
        }

        if (string.IsNullOrWhiteSpace(action.Input))
        {
            dispatcher.Dispatch(new SearchItemCategoriesFinishedAction([]));
            return Task.CompletedTask;
        }

        _itemCategoryStartSearchTimer = new(300d);
        _itemCategoryStartSearchTimer.AutoReset = false;
        _itemCategoryStartSearchTimer.Elapsed += (_, _) => dispatcher.Dispatch(new SearchItemCategoriesAction());
        _itemCategoryStartSearchTimer.Start();

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

    [EffectMethod]
    public Task HandleManufacturerInputChangedAction(ManufacturerInputChangedAction action, IDispatcher dispatcher)
    {
        if (_manufacturerStartSearchTimer is not null)
        {
            _manufacturerStartSearchTimer.Stop();
            _manufacturerStartSearchTimer.Dispose();
        }

        if (string.IsNullOrWhiteSpace(action.Input))
        {
            dispatcher.Dispatch(new SearchManufacturersFinishedAction([]));
            return Task.CompletedTask;
        }

        _manufacturerStartSearchTimer = new(300d);
        _manufacturerStartSearchTimer.AutoReset = false;
        _manufacturerStartSearchTimer.Elapsed += (_, _) => dispatcher.Dispatch(new SearchManufacturersAction());
        _manufacturerStartSearchTimer.Start();

        return Task.CompletedTask;
    }

    [EffectMethod(typeof(SearchManufacturersAction))]
    public async Task HandleSearchManufacturersAction(IDispatcher dispatcher)
    {
        var input = state.Value.Search.Filter.ManufacturerFilter.Input;
        if (string.IsNullOrWhiteSpace(input))
            return;

        IEnumerable<ManufacturerSearchResult> results;
        try
        {
            results = await client.GetManufacturerSearchResultsAsync(input);
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

        dispatcher.Dispatch(new SearchManufacturersFinishedAction([.. results]));
    }

    public ValueTask DisposeAsync()
    {
        _itemCategoryStartSearchTimer?.Dispose();
        _manufacturerStartSearchTimer?.Dispose();
        return ValueTask.CompletedTask;
    }
}