using ShoppingList.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Domain.Ports
{
    public interface IStoreRepository
    {
        Task<Store> FindByAsync(StoreId id, CancellationToken cancellationToken);
        Task<bool> IsValidIdAsync(StoreId id, CancellationToken cancellationToken);
        Task<StoreId> StoreAsync(Store store, CancellationToken cancellationToken);
    }
}