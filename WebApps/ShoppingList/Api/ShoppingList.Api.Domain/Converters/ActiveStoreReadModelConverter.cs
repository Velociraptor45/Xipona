using ShoppingList.Api.Domain.Models;
using ShoppingList.Api.Domain.Queries.AllActiveStores;
using System.Collections.Generic;

namespace ShoppingList.Api.Domain.Converters
{
    public static class ActiveStoreReadModelConverter
    {
        public static ActiveStoreReadModel ToActiveStoreReadModel(this Store model, IEnumerable<StoreItem> items)
        {
            return new ActiveStoreReadModel(model.Id, model.Name, items);
        }
    }
}