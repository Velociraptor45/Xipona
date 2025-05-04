using Fluxor;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Actions;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Actions.Settings;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports;
using ProjectHermes.Xipona.Frontend.Redux.Shared.States;
using RestEase;

namespace ProjectHermes.Xipona.Frontend.Redux.Shared.Effects;

public class SettingsEffects
{
    private readonly IApiClient _apiClient;
    private readonly IState<SharedState> _state;
    private readonly IShoppingListNotificationService _notificationService;

    public SettingsEffects(IApiClient apiClient, IState<SharedState> state,
        IShoppingListNotificationService notificationService)
    {
        _apiClient = apiClient;
        _state = state;
        _notificationService = notificationService;
    }

    [EffectMethod(typeof(OpenSettingsAction))]
    public async Task HandleOpenSettingsAction(IDispatcher dispatcher)
    {
        var currenciesTask = _apiClient.GetAllCurrenciesAsync();
        var generalSettingsTask = _apiClient.GetGeneralSettingsAsync();

        try
        {
            await Task.WhenAll(currenciesTask, generalSettingsTask);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Loading settings failed", e));
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Loading settings failed", e.Message));
            return;
        }

        dispatcher.Dispatch(new SettingsLoadedAction(generalSettingsTask.Result, currenciesTask.Result.ToList()));
    }

    [EffectMethod]
    public async Task HandleSaveSettingsAction(SaveSettingsAction action, IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new SaveSettingsStartedAction());

        try
        {
            await _apiClient.UpdateGeneralSettingsAsync(_state.Value.Settings.GeneralSettings.Currency);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Saving general settings failed", e));
            dispatcher.Dispatch(new SaveSettingsFinishedAction());
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Saving general settings failed", e.Message));
            dispatcher.Dispatch(new SaveSettingsFinishedAction());
            return;
        }

        dispatcher.Dispatch(new SaveSettingsFinishedAction());
        dispatcher.Dispatch(new CloseSettingsAction());
        _notificationService.NotifySuccess("Successfully saved general settings");
    }
}
