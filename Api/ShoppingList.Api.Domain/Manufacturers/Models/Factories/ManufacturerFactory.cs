namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models.Factories;

public class ManufacturerFactory : IManufacturerFactory
{
    public IManufacturer Create(ManufacturerId id, string name, bool isDeleted)
    {
        return new Manufacturer(id, name, isDeleted);
    }
}