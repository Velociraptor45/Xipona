using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemSearch;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services;

public interface IItemQueryService
{
    Task<IEnumerable<ItemSearchReadModel>> SearchAsync(string name, StoreId storeId);
}