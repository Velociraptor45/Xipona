using Xipona.Api.Domain.Common.Attributes;

namespace Xipona.Api.Domain.Common.Models;

public enum Currency
{
    [CurrencySymbol("€", true)]
    Euro = 0,
    [CurrencySymbol("$", false)]
    Dollar = 1,
    [CurrencySymbol("£", false)]
    Pound = 2,
    [CurrencySymbol("¥", false)]
    Yen = 3,
}
