using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Search;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Queries.SearchItemsForShoppingLists;

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