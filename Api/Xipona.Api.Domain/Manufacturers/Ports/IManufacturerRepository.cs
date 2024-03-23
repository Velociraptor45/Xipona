using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.Xipona.Api.Domain.Manufacturers.Ports;

public interface IManufacturerRepository
{
    Task<IManufacturer?> FindByAsync(ManufacturerId id);

    Task<IEnumerable<IManufacturer>> FindByAsync(string searchInput, bool includeDeleted);

    Task<IEnumerable<IManufacturer>> FindByAsync(IEnumerable<ManufacturerId> ids);

    Task<IEnumerable<IManufacturer>> FindActiveByAsync();

    Task<IManufacturer?> FindActiveByAsync(ManufacturerId id);

    Task<IManufacturer> StoreAsync(IManufacturer model);
}