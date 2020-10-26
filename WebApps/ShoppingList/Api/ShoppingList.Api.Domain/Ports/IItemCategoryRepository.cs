using ShoppingList.Api.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.Ports
{
    public interface IItemCategoryRepository
    {
        Task<IEnumerable<ItemCategory>> FindByAsync(string searchInput, CancellationToken cancellationToken);
        Task<ItemCategory> FindByAsync(ItemCategoryId id, CancellationToken cancellationToken);
        Task<IEnumerable<ItemCategory>> FindByAsync(IEnumerable<ItemCategoryId> ids, CancellationToken cancellationToken);
    }
}