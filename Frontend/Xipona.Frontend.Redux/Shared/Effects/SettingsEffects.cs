using Fluxor;
using Xipona.Frontend.Redux.Shared.Actions;
using Xipona.Frontend.Redux.Shared.Actions.Settings;
using Xipona.Frontend.Redux.Shared.Ports;
using Xipona.Frontend.Redux.Shared.States;
using RestEase;

namespace Xipona.Frontend.Redux.Shared.Effects;

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

    [EffectMethod(typeof(LoadGeneralSettingsAction))]
    public async Task HandleLoadGeneralSettingsAction(IDispatcher dispatcher)
    {
        if (_state.Value.Settings.GeneralSettings is not null)
            return;

        GeneralSettings generalSettings;
        try
        {
            generalSettings = await _apiClient.GetGeneralSettingsAsync();
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Loading general settings failed", e));
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Loading general settings failed", e.Message));
            return;
        }

        dispatcher.Dispatch(new GeneralSettingsLoadedAction(generalSettings));
    }

    [EffectMethod(typeof(OpenSettingsAction))]
    public async Task HandleOpenSettingsAction(IDispatcher dispatcher)
    {
        List<Currency> currencies;
        try
        {
            currencies = (await _apiClient.GetAllCurrenciesAsync()).ToList();
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

        dispatcher.Dispatch(new SettingsLoadedAction(currencies));
    }

    [EffectMethod]
    public async Task HandleSaveSettingsAction(SaveSettingsAction action, IDispatcher dispatcher)
    {
        if (_state.Value.Settings.Editor is null)
            return;

        dispatcher.Dispatch(new SaveSettingsStartedAction());

        try
        {
            await _apiClient.UpdateGeneralSettingsAsync(_state.Value.Settings.Editor!.GeneralSettings.Currency);
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
