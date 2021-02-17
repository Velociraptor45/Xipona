using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports
{
    public interface IItemCategoryRepository
    {
        Task<IEnumerable<IItemCategory>> FindByAsync(string searchInput, CancellationToken cancellationToken);

        Task<IItemCategory> FindByAsync(ItemCategoryId id, CancellationToken cancellationToken);

        Task<IEnumerable<IItemCategory>> FindByAsync(IEnumerable<ItemCategoryId> ids, CancellationToken cancellationToken);

        Task<IEnumerable<IItemCategory>> FindActiveByAsync(CancellationToken cancellationToken);

        Task<IItemCategory> StoreAsync(IItemCategory model, CancellationToken cancellationToken);
    }
}