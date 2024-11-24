using Fluxor;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Actions;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.Discounts;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;
using RestEase;

namespace ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Effects;
public class DiscountEffects
{
    private readonly IApiClient _apiClient;
    private readonly IState<ShoppingListState> _state;
    private readonly IShoppingListNotificationService _notificationService;

    public DiscountEffects(IApiClient apiClient, IState<ShoppingListState> state,
        IShoppingListNotificationService notificationService)
    {
        _apiClient = apiClient;
        _state = state;
        _notificationService = notificationService;
    }

    [EffectMethod(typeof(SaveDiscountAction))]
    public async Task HandleSaveDiscountAction(IDispatcher dispatcher)
    {
        var state = _state.Value;
        if (state.DiscountDialog.Item == null)
            return;

        var item = state.DiscountDialog.Item;
        var discount = state.DiscountDialog.Discount;

        dispatcher.Dispatch(new SaveDiscountStartedAction());

        try
        {
            await _apiClient.AddItemDiscountAsync(state.ShoppingList!.Id, item.Id.ActualId!.Value, item.TypeId, discount);
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
        _notificationService.NotifySuccess($"Successfully discounted {state.DiscountDialog.Item.Name}");
    }

    [EffectMethod(typeof(RemoveDiscountAction))]
    public async Task HandleRemoveDiscountAction(IDispatcher dispatcher)
    {
        var state = _state.Value;
        if (state.DiscountDialog.Item == null)
            return;

        var item = state.DiscountDialog.Item;

        dispatcher.Dispatch(new RemoveDiscountStartedAction());

        try
        {
            await _apiClient.RemoveItemDiscountAsync(state.ShoppingList!.Id, item.Id.ActualId!.Value, item.TypeId);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Removing discount failed", e));
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Removing discount failed", e.Message));
            return;
        }

        dispatcher.Dispatch(new RemoveDiscountFinishedAction());
        dispatcher.Dispatch(new CloseDiscountDialogAction());
        dispatcher.Dispatch(new ReloadCurrentShoppingListAction());
        _notificationService.NotifySuccess($"Successfully removed discount from {state.DiscountDialog.Item.Name}");
    }
}
