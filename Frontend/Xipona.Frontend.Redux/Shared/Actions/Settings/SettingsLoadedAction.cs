using Xipona.Frontend.Redux.Shared.States;

namespace Xipona.Frontend.Redux.Shared.Actions.Settings;

public record SettingsLoadedAction(IReadOnlyCollection<Currency> AllCurrencies);