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

    public ItemMergeEffects(IApiClient client)
    {
        _client = client;
    }

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
}
