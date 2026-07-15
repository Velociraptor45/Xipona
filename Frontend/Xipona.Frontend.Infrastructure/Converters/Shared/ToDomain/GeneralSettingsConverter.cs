using Xipona.Api.Contracts.Users.Queries.GetGeneralSettings;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.Shared.States;

namespace Xipona.Frontend.Infrastructure.Converters.Shared.ToDomain;

public class GeneralSettingsConverter : IToDomainConverter<GeneralSettingsContract, GeneralSettings>
{
    public GeneralSettings ToDomain(GeneralSettingsContract source)
    {
        return new GeneralSettings(new Currency(source.Currency.Id, source.Currency.Symbol, source.Currency.IsTrailing));
    }
}
