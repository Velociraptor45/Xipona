using Xipona.Api.Domain.Common.Attributes;

namespace Xipona.Api.Domain.Common.Models;

public enum Currency
{
    [CurrencySymbol("€")]
    Euro = 0,
    [CurrencySymbol("$")]
    Dollar = 1,
    [CurrencySymbol("£")]
    Pound = 2,
    [CurrencySymbol("¥")]
    Yen = 3,
}
