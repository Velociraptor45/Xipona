using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Ports
{
    public interface IStoreRepository
    {
        Task<IEnumerable<IStore>> FindActiveStoresAsync(CancellationToken cancellationToken);

        Task<IStore> FindByAsync(StoreId id, CancellationToken cancellationToken);

        Task<bool> IsValidIdAsync(StoreId id, CancellationToken cancellationToken);

        Task<StoreId> StoreAsync(IStore store, CancellationToken cancellationToken);
    }
}