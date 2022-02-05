using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services;

public interface IManufacturerValidationService
{
    Task ValidateAsync(ManufacturerId manufacturerId, CancellationToken cancellationToken);
}