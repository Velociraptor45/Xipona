using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.StoreQueries;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Stores.Queries.AllActiveStores;

public class AllActiveStoresQuery : IQuery<IEnumerable<StoreReadModel>>
{
}