using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemSearch;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services;

public interface IItemQueryService
{
    Task<IEnumerable<ItemForShoppingListSearchReadModel>> SearchAsync(string name, StoreId storeId);
}