using ShoppingList.Api.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.Ports
{
    public interface IStoreRepository
    {
        Task<IEnumerable<Store>> FindActiveStoresAsync(CancellationToken cancellationToken);

        Task<Store> FindByAsync(StoreId id, CancellationToken cancellationToken);

        Task<bool> IsValidIdAsync(StoreId id, CancellationToken cancellationToken);

        Task<StoreId> StoreAsync(Store store, CancellationToken cancellationToken);
    }
}