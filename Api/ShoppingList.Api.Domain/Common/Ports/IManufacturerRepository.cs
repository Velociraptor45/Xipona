using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Ports
{
    public interface IManufacturerRepository
    {
        Task<IManufacturer> FindByAsync(ManufacturerId id, CancellationToken cancellationToken);

        Task<IEnumerable<IManufacturer>> FindByAsync(string searchInput, CancellationToken cancellationToken);

        Task<IEnumerable<IManufacturer>> FindByAsync(IEnumerable<ManufacturerId> ids, CancellationToken cancellationToken);

        Task<IEnumerable<IManufacturer>> FindByAsync(bool includeDeleted, CancellationToken cancellationToken);

        Task<IManufacturer> StoreAsync(IManufacturer model, CancellationToken cancellationToken);
    }
}