using Xipona.Api.Domain.Manufacturers.Models;

namespace Xipona.Api.Domain.Manufacturers.Services.Creations;

public interface IManufacturerCreationService
{
    Task<IManufacturer> CreateAsync(ManufacturerName name);
}