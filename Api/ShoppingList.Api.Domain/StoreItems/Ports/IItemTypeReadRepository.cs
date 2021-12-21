using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports
{
    public interface IItemTypeReadRepository
    {
        Task<IEnumerable<ItemTypeId>> FindByAsync(string name, StoreId storeId, CancellationToken cancellationToken);
    }
}