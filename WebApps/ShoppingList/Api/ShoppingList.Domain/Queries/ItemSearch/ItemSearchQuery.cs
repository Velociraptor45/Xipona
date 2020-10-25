using ShoppingList.Domain.Models;
using System.Collections.Generic;

namespace ShoppingList.Domain.Queries.ItemSearch
{
    public class ItemSearchQuery : IQuery<IEnumerable<ItemSearchReadModel>>
    {
        public ItemSearchQuery(string searchInput, StoreId storeId)
        {
            SearchInput = searchInput;
            StoreId = storeId;
        }

        public string SearchInput { get; }
        public StoreId StoreId { get; }
    }
}