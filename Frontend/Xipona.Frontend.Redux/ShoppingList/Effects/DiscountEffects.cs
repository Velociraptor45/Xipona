using Fluxor;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Actions;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports;
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

    [EffectMethod]
    public async Task HandleSaveDiscountAction(SaveDiscountAction action, IDispatcher dispatcher)
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
        _notificationService.NotifySuccess($"{state.DiscountDialog.Item.Name} discounted");
    }
}
