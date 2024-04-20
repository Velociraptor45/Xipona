using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.GetActiveStoresOverview;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToContract.Stores;

public class StoreSearchResultContractConverter : IToContractConverter<IStore, StoreSearchResultContract>
{
    public StoreSearchResultContract ToContract(IStore source)
    {
        return new StoreSearchResultContract(source.Id, source.Name);
    }
}