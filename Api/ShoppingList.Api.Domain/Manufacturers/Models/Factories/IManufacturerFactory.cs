namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models.Factories;

public interface IManufacturerFactory
{
    IManufacturer Create(ManufacturerId id, ManufacturerName name, bool isDeleted);
}