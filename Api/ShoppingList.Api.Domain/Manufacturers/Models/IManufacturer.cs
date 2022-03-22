namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;

public interface IManufacturer
{
    ManufacturerId Id { get; }
    ManufacturerName Name { get; }
    bool IsDeleted { get; }
}