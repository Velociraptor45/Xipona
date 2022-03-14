using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Validations;

public interface IManufacturerValidationService
{
    Task ValidateAsync(ManufacturerId manufacturerId, CancellationToken cancellationToken);
}