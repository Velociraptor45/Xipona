using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Deletions;

public interface IManufacturerDeletionService
{
    Task DeleteAsync(ManufacturerId manufacturerId);
}