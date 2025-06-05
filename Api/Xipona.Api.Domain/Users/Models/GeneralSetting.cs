using Xipona.Api.Core.Extensions;
using Xipona.Api.Domain.Common.Attributes;
using Xipona.Api.Domain.Common.Models;

namespace Xipona.Api.Domain.Users.Models;

public class GeneralSetting : AggregateRoot, IGeneralSetting
{
    public GeneralSetting(GeneralSettingId id, Currency currency)
    {
        Id = id;
        Currency = currency;
        CurrencySymbol = GetCurrencySymbol();
    }

    public GeneralSettingId Id { get; }
    public Currency Currency { get; private set; }
    public string CurrencySymbol { get; private set; }

    public void Update(Currency currency)
    {
        Currency = currency;
        CurrencySymbol = GetCurrencySymbol();
    }

    private string GetCurrencySymbol()
    {
        return Currency.GetAttribute<CurrencySymbolAttribute>().Symbol;
    }
}
