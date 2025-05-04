using ProjectHermes.Xipona.Api.Contracts.Users.Queries.GetGeneralSettings;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Domain.Common.Attributes;
using ProjectHermes.Xipona.Api.Domain.Users.Models;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToContract.Users;

public class GeneralSettingsContractConverter : IToContractConverter<IGeneralSetting, GeneralSettingsContract>
{
    public GeneralSettingsContract ToContract(IGeneralSetting source)
    {
        return new GeneralSettingsContract(
            new CurrencyContract(
                source.Currency.ToInt(),
                source.Currency.GetAttribute<CurrencySymbolAttribute>().Symbol));
    }
}
