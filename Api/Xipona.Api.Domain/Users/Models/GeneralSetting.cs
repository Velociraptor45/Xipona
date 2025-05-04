using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Domain.Common.Attributes;
using ProjectHermes.Xipona.Api.Domain.Common.Models;

namespace ProjectHermes.Xipona.Api.Domain.Users.Models;

public class GeneralSetting : AggregateRoot, IGeneralSetting
{
    public GeneralSetting(GeneralSettingId id, Currency currency)
    {
        Id = id;
        Currency = currency;
        CurrencySymbol = currency.GetAttribute<CurrencySymbolAttribute>().Symbol;
    }

    public GeneralSettingId Id { get; }
    public Currency Currency { get; private set; }
    public string CurrencySymbol { get; }

    public void Update(Currency currency)
    {
        Currency = currency;
    }
}
