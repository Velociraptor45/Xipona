using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Searches;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Queries.SearchItems;

public class SearchItemQuery : IQuery<IEnumerable<SearchItemResultReadModel>>
{
    public SearchItemQuery(string searchInput)
    {
        SearchInput = searchInput;
    }

    public string SearchInput { get; }
}