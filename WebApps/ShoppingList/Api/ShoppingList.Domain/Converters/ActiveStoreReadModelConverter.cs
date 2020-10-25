using ShoppingList.Domain.Models;
using ShoppingList.Domain.Queries.AllActiveStores;
using System.Collections.Generic;

namespace ShoppingList.Domain.Converters
{
    public static class ActiveStoreReadModelConverter
    {
        public static ActiveStoreReadModel ToActiveStoreReadModel(this Store model, IEnumerable<StoreItem> items)
        {
            return new ActiveStoreReadModel(model.Id, model.Name, items);
        }
    }
}