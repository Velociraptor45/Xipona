using Fluxor;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Actions.Settings;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports;
using ProjectHermes.Xipona.Frontend.Redux.Shared.States;

namespace ProjectHermes.Xipona.Frontend.Redux.Shared.Effects;

public class SettingsEffects
{
    private readonly IApiClient _apiClient;

    public SettingsEffects(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [EffectMethod(typeof(OpenSettingsAction))]
    public async Task HandleOpenSettingsAction(IDispatcher dispatcher)
    {
        var currencies = (await _apiClient.GetAllCurrenciesAsync()).ToList();
        var generalSettings = new GeneralSettings(currencies, currencies[0]); //todo load from settings
        dispatcher.Dispatch(new SettingsLoadedAction(generalSettings));
    }
}
