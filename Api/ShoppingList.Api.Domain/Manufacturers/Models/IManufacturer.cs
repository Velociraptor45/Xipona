namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;

public interface IManufacturer
{
    ManufacturerId Id { get; }
    string Name { get; }
    bool IsDeleted { get; }
}