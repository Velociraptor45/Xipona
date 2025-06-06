using Xipona.Api.Contracts.Users.Commands.AllCurrencies;
using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Users.Services.Queries;

namespace Xipona.Api.Endpoint.v1.Converters.ToContract.Users;

public class CurrencyContractConverter : IToContractConverter<CurrencyReadModel, CurrencyContract>
{
    public CurrencyContract ToContract(CurrencyReadModel source)
    {
        return new CurrencyContract(source.Id, source.Symbol);
    }
}
