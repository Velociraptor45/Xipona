using Fluxor;
using ProjectHermes.ShoppingList.Api.Contracts.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Shared.Effects;

public class NotificationEffects
{
    private readonly IShoppingListNotificationService _notificationService;

    public NotificationEffects(IShoppingListNotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [EffectMethod]
    public async Task HandleDisplayErrorNotificationAction(DisplayErrorNotificationAction action, IDispatcher dispatcher)
    {
        await _notificationService.NotifyErrorAsync(action.Title, action.Message);
    }

    [EffectMethod]
    public async Task HandleDisplayApiExceptionNotificationAction(DisplayApiExceptionNotificationAction action,
        IDispatcher dispatcher)
    {
        var contract = action.Exception.DeserializeContent<ErrorContract>();

        await _notificationService.NotifyErrorAsync(action.Title, contract.Message);
    }

    [EffectMethod]
    public async Task HandleDisplayUnhandledErrorAction(DisplayUnhandledErrorAction action, IDispatcher dispatcher)
    {
        await _notificationService.NotifyErrorAsync("An error occurred", action.Message);
    }
}