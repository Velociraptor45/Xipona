using ShoppingList.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Domain.Ports
{
    public interface IManufacturerRepository
    {
        Task<Manufacturer> FindByAsync(ManufacturerId id, CancellationToken cancellationToken);
        Task<IEnumerable<Manufacturer>> FindByAsync(string searchInput, CancellationToken cancellationToken);
    }
}