using Fluxor;
using Microsoft.AspNetCore.Components;
using RestEase;
using Xipona.Frontend.Redux.Items.Actions.Merges;
using Xipona.Frontend.Redux.Items.States;
using Xipona.Frontend.Redux.Shared.Actions;
using Xipona.Frontend.Redux.Shared.Constants;
using Xipona.Frontend.Redux.Shared.Ports;

namespace Xipona.Frontend.Redux.Items.Effects;
public class ItemMergeEffects
{
    private readonly IApiClient _client;
    private readonly IState<ItemState> _state;
    private readonly NavigationManager _navigationManager;

    public ItemMergeEffects(IApiClient client, IState<ItemState> state, NavigationManager navigationManager)
    {
        _client = client;
        _state = state;
        _navigationManager = navigationManager;
    }

    [EffectMethod(typeof(EnterMergerAction))]
    public Task HandleEnterMergerAction(IDispatcher dispatcher)
    {
        if (_state.Value.Merge.Selector.SelectedItems.Count == 0 || _state.Value.Editor.Item is null)
            return Task.CompletedTask;

        var itemIds = _state.Value.Merge.Selector.SelectedItems.Select(i => i.Id).ToList();
        itemIds.Add(_state.Value.Editor.Item.Id);

        var uri = _navigationManager.GetUriWithQueryParameters("/items/merge", new Dictionary<string, object?>
        {
            { "itemId", itemIds.ToArray() }
        });
        _navigationManager.NavigateTo(uri);

        return Task.CompletedTask;
    }

    [EffectMethod]
    public async Task HandleInitializeMergingAction(InitializeMergingAction action, IDispatcher dispatcher)
    {
        List<EditedItem> items = new();

        try
        {
            var tasks = action.ItemIds.Distinct().Select(_client.GetItemByIdAsync).ToList();
            await Task.WhenAll(tasks);
            foreach (var task in tasks)
            {
                var item = await task;
                items.Add(item);
            }
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Loading item failed", e));
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Loading item failed", e.Message));
        }

        dispatcher.Dispatch(new InitializeMergingFinishedAction(items));
    }

    [EffectMethod(typeof(MergeItemsAction))]
    public async Task HandleMergeItemsAction(IDispatcher dispatcher)
    {
        if (_state.Value.Merge.Item is null)
            return;

        dispatcher.Dispatch(new MergeItemsStartedAction());

        Guid newItemId;
        try
        {
            newItemId = await _client.MergeItemsAsync(_state.Value.Merge.Item);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Merging items failed", e));
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Merging items failed", e.Message));
            return;
        }
        finally
        {
            dispatcher.Dispatch(new MergeItemsFinishedAction());
        }

        dispatcher.Dispatch(new LeaveItemMergerAction(newItemId));
    }

    [EffectMethod]
    public Task HandleLeaveItemMergerAction(LeaveItemMergerAction action, IDispatcher dispatcher)
    {
        if (action.NewItemId is null)
        {
            var item = _state.Value.Editor.Item;
            _navigationManager.NavigateTo(item is null
                ? PageRoutes.Items
                : $"{PageRoutes.Items}/{item.Id}");
        }
        else
        {
            _navigationManager.NavigateTo($"{PageRoutes.Items}/{action.NewItemId.Value}");
        }

        return Task.CompletedTask;
    }

    [EffectMethod(typeof(OpenMergeItemSelectorAction))]
    public async Task HandleOpenMergeItemSelectorAction(IDispatcher dispatcher)
    {
        if (_state.Value.Editor.Item is null)
            return;

        var item = _state.Value.Editor.Item;

        dispatcher.Dispatch(new SearchItemsForMergeStartedAction());

        try
        {
            var searchResults = await _client.SearchItemsForMergeAsync(item, [item.Id]);
            dispatcher.Dispatch(new SearchItemsForMergeFinishedAction(searchResults.ToList()));
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Searching for mergable items failed", e));
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Searching for mergable items failed", e.Message));
        }
    }
}
