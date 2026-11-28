using Xipona.Api.ApplicationServices.Common.Queries;
using Xipona.Api.Domain.Items.Services.Searches;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.ApplicationServices.Items.Queries.SearchItemsForShoppingLists;

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