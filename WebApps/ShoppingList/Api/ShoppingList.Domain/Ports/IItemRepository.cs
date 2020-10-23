using ShoppingList.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Domain.Ports
{
    public interface IItemRepository
    {
        Task<StoreItem> FindByAsync(StoreItemId storeItemId, StoreId storeId, CancellationToken cancellationToken);

        Task<bool> IsValidIdAsync(StoreItemId id, CancellationToken cancellationToken);

        Task<StoreItemId> StoreAsync(StoreItem storeItem, CancellationToken cancellationToken);
    }
}