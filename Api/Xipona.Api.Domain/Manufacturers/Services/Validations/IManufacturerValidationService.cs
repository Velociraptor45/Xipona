using Xipona.Api.Domain.Manufacturers.Models;

namespace Xipona.Api.Domain.Manufacturers.Services.Validations;

public interface IManufacturerValidationService
{
    Task ValidateAsync(ManufacturerId manufacturerId);
}