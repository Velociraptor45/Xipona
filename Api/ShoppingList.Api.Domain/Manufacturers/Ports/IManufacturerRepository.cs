using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;

public interface IManufacturerRepository
{
    Task<IManufacturer?> FindByAsync(ManufacturerId id, CancellationToken cancellationToken);

    Task<IEnumerable<IManufacturer>> FindByAsync(string searchInput, bool includeDeleted,
        CancellationToken cancellationToken);

    Task<IEnumerable<IManufacturer>> FindByAsync(IEnumerable<ManufacturerId> ids, CancellationToken cancellationToken);

    Task<IEnumerable<IManufacturer>> FindByAsync(bool includeDeleted, CancellationToken cancellationToken);

    Task<IManufacturer> StoreAsync(IManufacturer model, CancellationToken cancellationToken);
}