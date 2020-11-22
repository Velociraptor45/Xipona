using ShoppingList.Api.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.Ports
{
    public interface IManufacturerRepository
    {
        Task<Manufacturer> FindByAsync(ManufacturerId id, CancellationToken cancellationToken);
        Task<IEnumerable<Manufacturer>> FindByAsync(string searchInput, CancellationToken cancellationToken);
        Task<IEnumerable<Manufacturer>> FindByAsync(IEnumerable<ManufacturerId> ids, CancellationToken cancellationToken);
        Task<IEnumerable<Manufacturer>> FindByAsync(bool includeDeleted, CancellationToken cancellationToken);
        Task<Manufacturer> StoreAsync(Manufacturer model, CancellationToken cancellationToken);
    }
}