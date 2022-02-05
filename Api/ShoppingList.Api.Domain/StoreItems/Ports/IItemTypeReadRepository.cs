using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;

public interface IItemTypeReadRepository
{
    Task<IEnumerable<(ItemId, ItemTypeId)>> FindActiveByAsync(string name, StoreId storeId, CancellationToken cancellationToken);
}