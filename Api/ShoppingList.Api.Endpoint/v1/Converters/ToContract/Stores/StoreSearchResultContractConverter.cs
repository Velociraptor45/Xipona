using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.GetActiveStoresOverview;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.Stores;

public class StoreSearchResultContractConverter : IToContractConverter<IStore, StoreSearchResultContract>
{
    public StoreSearchResultContract ToContract(IStore source)
    {
        return new StoreSearchResultContract(source.Id, source.Name);
    }
}