using Xipona.Api.Contracts.Users.Queries.GetGeneralSettings;
using Xipona.Api.Core.Converter;
using Xipona.Api.Core.Extensions;
using Xipona.Api.Domain.Common.Attributes;
using Xipona.Api.Domain.Users.Models;

namespace Xipona.Api.Endpoint.v1.Converters.ToContract.Users;

public class GeneralSettingsContractConverter : IToContractConverter<IGeneralSetting, GeneralSettingsContract>
{
    public GeneralSettingsContract ToContract(IGeneralSetting source)
    {
        var attr = source.Currency.GetAttribute<CurrencySymbolAttribute>();
        
        return new GeneralSettingsContract(
            new CurrencyContract(
                source.Currency.ToInt(),
                attr.Symbol,
                attr.IsTrailing));
    }
}
