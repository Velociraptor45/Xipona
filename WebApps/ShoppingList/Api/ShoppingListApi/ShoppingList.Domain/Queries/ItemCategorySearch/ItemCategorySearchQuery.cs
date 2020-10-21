using ShoppingList.Domain.Queries.SharedModels;
using System.Collections.Generic;

namespace ShoppingList.Domain.Queries.ItemCategorySearch
{
    public class ItemCategorySearchQuery : IQuery<IEnumerable<ItemCategoryReadModel>>
    {
        public ItemCategorySearchQuery(string searchInput)
        {
            SearchInput = searchInput;
        }

        public string SearchInput { get; }
    }
}