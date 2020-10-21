using ShoppingList.Domain.Queries.SharedModels;
using System.Collections.Generic;

namespace ShoppingList.Domain.Queries.AllActiveStores
{
    public class AllActiveStoresQuery : IQuery<IEnumerable<StoreReadModel>>
    {
    }
}