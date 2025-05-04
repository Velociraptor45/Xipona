using Fluxor;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Actions.Settings;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports;

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
        var currenciesTask = _apiClient.GetAllCurrenciesAsync();
        var generalSettingsTask = _apiClient.GetGeneralSettingsAsync();

        await Task.WhenAll(currenciesTask, generalSettingsTask);

        dispatcher.Dispatch(new SettingsLoadedAction(generalSettingsTask.Result, currenciesTask.Result.ToList()));
    }
}
