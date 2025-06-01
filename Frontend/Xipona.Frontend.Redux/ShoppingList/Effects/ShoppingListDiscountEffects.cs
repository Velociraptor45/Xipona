using Fluxor;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Actions;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.ShoppingListDiscounts;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;
using RestEase;

namespace ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Effects;

public class ShoppingListDiscountEffects
{
    private readonly IApiClient _client;
    private readonly IState<ShoppingListState> _state;

    public ShoppingListDiscountEffects(IApiClient client, IState<ShoppingListState> state)
    {
        _client = client;
        _state = state;
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
    }
}
