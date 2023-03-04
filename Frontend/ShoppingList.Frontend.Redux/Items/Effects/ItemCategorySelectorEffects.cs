using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.Actions.Editor.ItemCategorySelectors;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;
using RestEase;
using Timer = System.Timers.Timer;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Items.Effects;

public sealed class ItemCategorySelectorEffects : IDisposable
{
    private readonly IApiClient _client;
    private readonly IState<ItemState> _state;

    private Timer? _startSearchTimer;

    public ItemCategorySelectorEffects(IApiClient client, IState<ItemState> state)
    {
        _client = client;
        _state = state;
    }

    [EffectMethod(typeof(LoadInitialItemCategoryAction))]
    public async Task HandleLoadInitialItemCategoryAction(IDispatcher dispatcher)
    {
        if (_state.Value.Editor.Item?.ItemCategoryId is null)
            return;

        EditedItemCategory itemCategory;

        try
        {
            itemCategory = await _client.GetItemCategoryByIdAsync(_state.Value.Editor.Item!.ItemCategoryId!.Value);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Loading item category failed", e));
            return;
        }

        var result = new ItemCategorySearchResult(itemCategory.Id, itemCategory.Name);
        dispatcher.Dispatch(new LoadInitialItemCategoryFinishedAction(result));
    }

    [EffectMethod(typeof(CreateNewItemCategoryAction))]
    public async Task HandleCreateNewItemCategoryAction(IDispatcher dispatcher)
    {
        EditedItemCategory result;
        try
        {
            result = await _client.CreateItemCategoryAsync(_state.Value.Editor.ItemCategorySelector.Input);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Creating item category failed", e));
            return;
        }

        dispatcher.Dispatch(
            new CreateNewItemCategoryFinishedAction(new ItemCategorySearchResult(result.Id, result.Name)));
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
            dispatcher.Dispatch(new SearchItemCategoryFinishedAction(new List<ItemCategorySearchResult>()));
            return Task.CompletedTask;
        }

        _startSearchTimer = new(300d);
        _startSearchTimer.AutoReset = false;
        _startSearchTimer.Elapsed += (_, _) => dispatcher.Dispatch(new SearchItemCategoryAction());
        _startSearchTimer.Start();

        return Task.CompletedTask;
    }

    [EffectMethod(typeof(ItemCategoryDropdownClosedAction))]
    public static Task HandleItemCategoryDropdownClosedAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new ItemCategoryInputChangedAction(string.Empty));
        return Task.CompletedTask;
    }

    [EffectMethod(typeof(SearchItemCategoryAction))]
    public async Task HandleSearchItemCategoryAction(IDispatcher dispatcher)
    {
        var input = _state.Value.Editor.ItemCategorySelector.Input;
        if (string.IsNullOrWhiteSpace(input))
            return;

        IEnumerable<ItemCategorySearchResult> result;
        try
        {
            result = await _client.GetItemCategorySearchResultsAsync(input);
        }
        catch (OperationCanceledException)
        {
            return;
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Searching for item categories failed", e));
            return;
        }
        dispatcher.Dispatch(new SearchItemCategoryFinishedAction(result.ToList()));
    }

    public void Dispose()
    {
        _startSearchTimer?.Dispose();
    }
}