namespace ProjectHermes.Xipona.Frontend.Redux.Shared.States;

public record GeneralSettings(IReadOnlyCollection<Currency> AllCurrencies, Currency Currency);