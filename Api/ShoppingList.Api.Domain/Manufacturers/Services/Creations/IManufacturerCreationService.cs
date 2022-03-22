using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Creations;

public interface IManufacturerCreationService
{
    Task CreateAsync(ManufacturerName name);
}