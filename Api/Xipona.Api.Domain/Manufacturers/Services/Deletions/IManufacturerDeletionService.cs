using Xipona.Api.Domain.Manufacturers.Models;

namespace Xipona.Api.Domain.Manufacturers.Services.Deletions;

public interface IManufacturerDeletionService
{
    Task DeleteAsync(ManufacturerId manufacturerId);
}