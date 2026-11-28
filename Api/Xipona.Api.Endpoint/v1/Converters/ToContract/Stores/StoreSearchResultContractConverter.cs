using Xipona.Api.Contracts.Stores.Queries.GetActiveStoresOverview;
using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Endpoint.v1.Converters.ToContract.Stores;

public class StoreSearchResultContractConverter : IToContractConverter<IStore, StoreSearchResultContract>
{
    public StoreSearchResultContract ToContract(IStore source)
    {
        return new StoreSearchResultContract(source.Id, source.Name);
    }
}