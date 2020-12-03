using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Queries.AllActiveStores
{
    public class AllActiveStoresQuery : IQuery<IEnumerable<ActiveStoreReadModel>>
    {
    }
}