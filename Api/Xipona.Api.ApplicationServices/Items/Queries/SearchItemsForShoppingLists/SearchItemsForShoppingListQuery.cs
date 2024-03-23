using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Searches;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Items.Queries.SearchItemsForShoppingLists;

public class SearchItemsForShoppingListQuery : IQuery<IEnumerable<SearchItemForShoppingResultReadModel>>
{
    public SearchItemsForShoppingListQuery(string searchInput, StoreId storeId)
    {
        SearchInput = searchInput;
        StoreId = storeId;
    }

    public string SearchInput { get; }
    public StoreId StoreId { get; }
}