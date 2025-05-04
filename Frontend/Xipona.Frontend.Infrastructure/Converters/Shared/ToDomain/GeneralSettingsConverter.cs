using ProjectHermes.Xipona.Api.Contracts.Users.Queries.GetGeneralSettings;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.Shared.States;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Shared.ToDomain;

public class GeneralSettingsConverter : IToDomainConverter<GeneralSettingsContract, GeneralSettings>
{
    public GeneralSettings ToDomain(GeneralSettingsContract source)
    {
        return new GeneralSettings(new Currency(source.Currency.Id, source.Currency.Symbol));
    }
}
