namespace ProjectHermes.Xipona.Frontend.Redux.Shared.States;

public record SettingsEditor(GeneralSettings GeneralSettings, IReadOnlyCollection<Currency> AllCurrencies);