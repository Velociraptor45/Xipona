using Fluxor;
using ProjectHermes.Xipona.Api.Contracts.Common;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Actions;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports;

namespace ProjectHermes.Xipona.Frontend.Redux.Shared.Effects;

public class NotificationEffects
{
    private readonly IShoppingListNotificationService _notificationService;

    public NotificationEffects(IShoppingListNotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [EffectMethod]
    public Task HandleDisplayErrorNotificationAction(DisplayErrorNotificationAction action, IDispatcher dispatcher)
    {
        _notificationService.NotifyError(action.Title, action.Message);
        return Task.CompletedTask;
    }

    [EffectMethod]
    public Task HandleDisplayApiExceptionNotificationAction(DisplayApiExceptionNotificationAction action,
        IDispatcher dispatcher)
    {
        var contract = action.Exception.DeserializeContent<ErrorContract>();
        if (contract is null)
        {
            _notificationService.NotifyError(action.Title, action.Exception.Message);
            return Task.CompletedTask;
        }

        _notificationService.NotifyError(action.Title, contract.Message);
        return Task.CompletedTask;
    }

    [EffectMethod]
    public Task HandleDisplayUnhandledErrorAction(DisplayUnhandledErrorAction action, IDispatcher dispatcher)
    {
        _notificationService.NotifyError("An error occurred", action.Message);
        return Task.CompletedTask;
    }
}