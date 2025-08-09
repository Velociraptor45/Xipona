using Fluxor;
using Microsoft.AspNetCore.Components;
using RestEase;
using Xipona.Frontend.Redux.Items.Actions.Merges;
using Xipona.Frontend.Redux.Items.States;
using Xipona.Frontend.Redux.Shared.Actions;
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
        if (_state.Value.Merge.Selector.SelectedItems.Count == 0)
            return Task.CompletedTask;

        var uri = _navigationManager.GetUriWithQueryParameters("/items/merge", new Dictionary<string, object?>
        {
            { "itemId", _state.Value.Merge.Selector.SelectedItems.Select(i => i.Id) }
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

        try
        {
            await _client.MergeItemsAsync(_state.Value.Merge.Item);
            dispatcher.Dispatch(new MergeItemsFinishedAction());
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Merging items failed", e));
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Merging items failed", e.Message));
        }
    }
}
