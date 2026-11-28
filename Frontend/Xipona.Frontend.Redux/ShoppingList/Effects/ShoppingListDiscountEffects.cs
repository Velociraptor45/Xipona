using Fluxor;
using Xipona.Frontend.Redux.Shared.Actions;
using Xipona.Frontend.Redux.Shared.Ports;
using Xipona.Frontend.Redux.ShoppingList.Actions;
using Xipona.Frontend.Redux.ShoppingList.Actions.ShoppingListDiscounts;
using Xipona.Frontend.Redux.ShoppingList.States;
using RestEase;

namespace Xipona.Frontend.Redux.ShoppingList.Effects;

public class ShoppingListDiscountEffects
{
    private readonly IApiClient _client;
    private readonly IState<ShoppingListState> _state;
    private readonly IShoppingListNotificationService _notificationService;

    public ShoppingListDiscountEffects(IApiClient client, IState<ShoppingListState> state,
        IShoppingListNotificationService notificationService)
    {
        _client = client;
        _state = state;
        _notificationService = notificationService;
    }

    [EffectMethod(typeof(SaveDiscountAction))]
    public async Task HandleSaveDiscountAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new SaveDiscountStartedAction());

        try
        {
            await _client.AddShoppingListDiscountAsync(
                _state.Value.ShoppingList!.Id,
                _state.Value.ShoppingListDiscountDialog.DiscountValue,
                _state.Value.ShoppingListDiscountDialog.Type);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Saving discount failed", e));
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Saving discount failed", e.Message));
            return;
        }

        dispatcher.Dispatch(new SaveDiscountFinishedAction());
        dispatcher.Dispatch(new CloseDiscountDialogAction());
        dispatcher.Dispatch(new ReloadCurrentShoppingListAction());
        _notificationService.NotifySuccess("Successfully added discount");
    }

    [EffectMethod]
    public async Task HandleRemoveDiscountAction(RemoveDiscountAction action, IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new RemoveDiscountStartedAction(action.DiscountId));

        try
        {
            await _client.RemoveShoppingListDiscountAsync(_state.Value.ShoppingList!.Id, action.DiscountId);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Removing discount failed", e));
            dispatcher.Dispatch(new RemoveDiscountFailedAction(action.DiscountId));
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Removing discount failed", e.Message));
            dispatcher.Dispatch(new RemoveDiscountFailedAction(action.DiscountId));
            return;
        }

        dispatcher.Dispatch(new RemoveDiscountFinishedAction(action.DiscountId));
        _notificationService.NotifySuccess("Successfully removed discount");
    }
}
