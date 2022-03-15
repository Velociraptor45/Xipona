namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Creations;

public interface IManufacturerCreationService
{
    Task CreateAsync(string name);
}