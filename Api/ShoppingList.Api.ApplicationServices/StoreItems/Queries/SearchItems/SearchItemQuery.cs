using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Search;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Queries.SearchItems
{
    public class SearchItemQuery : IQuery<IEnumerable<SearchItemResultReadModel>>
    {
        public SearchItemQuery(string searchInput)
        {
            SearchInput = searchInput;
        }

        public string SearchInput { get; }
    }
}