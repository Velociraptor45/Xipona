using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemSearch;

public class SearchItemForShoppingListQuery : IQuery<IEnumerable<ItemForShoppingListSearchReadModel>>
{
    public SearchItemForShoppingListQuery(string searchInput, StoreId storeId)
    {
        SearchInput = searchInput;
        StoreId = storeId;
    }

    public string SearchInput { get; }
    public StoreId StoreId { get; }
}