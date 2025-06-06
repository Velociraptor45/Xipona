using Xipona.Api.Contracts.Users.Commands.AllCurrencies;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.Shared.States;

namespace Xipona.Frontend.Infrastructure.Converters.Shared.ToDomain;

public class CurrencyConverter : IToDomainConverter<CurrencyContract, Currency>
{
    public Currency ToDomain(CurrencyContract source)
    {
        return new(source.Id, source.Symbol);
    }
}
