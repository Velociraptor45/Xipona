using ProjectHermes.Xipona.Frontend.Redux.Shared.States;

namespace ProjectHermes.Xipona.Frontend.Redux.Shared.Actions.Settings;

public record SettingsLoadedAction(GeneralSettings GeneralSettings, IReadOnlyCollection<Currency> AllCurrencies);