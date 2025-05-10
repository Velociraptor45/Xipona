using ProjectHermes.Xipona.Api.Contracts.Users.Commands.AllCurrencies;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.Shared.States;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Shared.ToDomain;

public class CurrencyConverter : IToDomainConverter<CurrencyContract, Currency>
{
    public Currency ToDomain(CurrencyContract source)
    {
        return new(source.Id, source.Symbol);
    }
}
