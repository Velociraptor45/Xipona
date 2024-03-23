using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Deletions;

public interface IManufacturerDeletionService
{
    Task DeleteAsync(ManufacturerId manufacturerId);
}