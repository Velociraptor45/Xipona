using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemFilterResults;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Queries.SearchItems
{
    public class SearchItemQuery : IQuery<IEnumerable<ItemFilterResultReadModel>>
    {
        public SearchItemQuery(string searchInput)
        {
            SearchInput = searchInput;
        }

        public string SearchInput { get; }
    }
}