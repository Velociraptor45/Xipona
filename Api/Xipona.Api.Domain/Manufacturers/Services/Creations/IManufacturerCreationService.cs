using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Creations;

public interface IManufacturerCreationService
{
    Task<IManufacturer> CreateAsync(ManufacturerName name);
}