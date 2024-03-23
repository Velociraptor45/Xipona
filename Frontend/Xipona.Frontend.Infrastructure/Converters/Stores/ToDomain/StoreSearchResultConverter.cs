using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.GetActiveStoresOverview;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.Stores.States;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Stores.ToDomain;

public class StoreSearchResultConverter : IToDomainConverter<StoreSearchResultContract, StoreSearchResult>
{
    public StoreSearchResult ToDomain(StoreSearchResultContract source)
    {
        return new StoreSearchResult(source.Id, source.Name);
    }
}