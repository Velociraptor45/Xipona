using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Ports
{
    public interface IItemCategoryRepository
    {
        Task<IEnumerable<ItemCategory>> FindByAsync(string searchInput, CancellationToken cancellationToken);
        Task<ItemCategory> FindByAsync(ItemCategoryId id, CancellationToken cancellationToken);
        Task<IEnumerable<ItemCategory>> FindByAsync(IEnumerable<ItemCategoryId> ids, CancellationToken cancellationToken);
        Task<IEnumerable<ItemCategory>> FindByAsync(bool includeDeleted, CancellationToken cancellationToken);
        Task<ItemCategory> StoreAsync(ItemCategory model, CancellationToken cancellationToken);
    }
}