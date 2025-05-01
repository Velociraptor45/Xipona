using ProjectHermes.Xipona.Api.Domain.Common.Attributes;

namespace ProjectHermes.Xipona.Api.Domain.Common.Models;

public enum Currency
{
    [CurrencySign("€")]
    Euro = 0,
    [CurrencySign("$")]
    Dollar = 1,
    [CurrencySign("£")]
    Pound = 2,
    [CurrencySign("¥")]
    Yen = 3,
}
