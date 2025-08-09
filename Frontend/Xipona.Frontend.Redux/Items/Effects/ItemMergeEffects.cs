using Fluxor;
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

    public ItemMergeEffects(IApiClient client, IState<ItemState> state)
    {
        _client = client;
        _state = state;
    }

    [EffectMethod]
    public async Task HandleInitializeMergingAction(InitializeMergingAction action, IDispatcher dispatcher)
    {
        List<EditedItem> items = new();

        var tasks = action.ItemIds.Select(_client.GetItemByIdAsync).ToList();

        try
        {
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
