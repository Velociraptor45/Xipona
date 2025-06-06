using Xipona.Api.Contracts.Stores.Queries.GetActiveStoresOverview;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.Stores.States;

namespace Xipona.Frontend.Infrastructure.Converters.Stores.ToDomain;

public class StoreSearchResultConverter : IToDomainConverter<StoreSearchResultContract, StoreSearchResult>
{
    public StoreSearchResult ToDomain(StoreSearchResultContract source)
    {
        return new StoreSearchResult(source.Id, source.Name);
    }
}