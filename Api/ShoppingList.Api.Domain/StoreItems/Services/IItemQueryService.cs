using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemSearch;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services
{
    public interface IItemQueryService
    {
        Task<IEnumerable<ItemSearchReadModel>> SearchAsync(string name, StoreId storeId);
    }
}