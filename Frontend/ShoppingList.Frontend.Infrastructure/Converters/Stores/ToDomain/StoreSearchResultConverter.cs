using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.GetActiveStoresOverview;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.Stores.States;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Stores.ToDomain;

public class StoreSearchResultConverter : IToDomainConverter<StoreSearchResultContract, StoreSearchResult>
{
    public StoreSearchResult ToDomain(StoreSearchResultContract source)
    {
        return new StoreSearchResult(source.Id, source.Name);
    }
}