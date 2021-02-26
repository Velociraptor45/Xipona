using ProjectHermes.ShoppingList.Api.Domain.Stores.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Ports
{
    public interface IStoreRepository
    {
        Task<IEnumerable<IStore>> GetAsync(CancellationToken cancellationToken);

        Task<IStore> FindByAsync(StoreId id, CancellationToken cancellationToken);

        Task StoreAsync(IStore store, CancellationToken cancellationToken);

        Task<IStore> FindActiveByAsync(StoreId id, CancellationToken cancellationToken);
    }
}