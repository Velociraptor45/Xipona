using ProjectHermes.Xipona.Api.Contracts.Users.Commands.AllCurrencies;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Users.Services.Queries;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToContract.Users;

public class CurrencyContractConverter : IToContractConverter<CurrencyReadModel, CurrencyContract>
{
    public CurrencyContract ToContract(CurrencyReadModel source)
    {
        return new CurrencyContract(source.Id, source.Symbol);
    }
}
