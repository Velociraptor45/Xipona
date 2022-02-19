using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemFilterResults;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemSearchForShoppingLists;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services;

public interface IItemQueryService
{
    Task<IEnumerable<ItemForShoppingListSearchReadModel>> SearchForShoppingListAsync(string name, StoreId storeId);

    Task<IEnumerable<ItemFilterResultReadModel>> SearchAsync(string searchInput);
}