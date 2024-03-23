using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Validations;

public interface IManufacturerValidationService
{
    Task ValidateAsync(ManufacturerId manufacturerId);
}