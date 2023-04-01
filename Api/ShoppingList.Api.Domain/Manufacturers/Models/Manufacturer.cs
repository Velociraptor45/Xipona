using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Modifications;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;

public class Manufacturer : AggregateRoot, IManufacturer
{
    public Manufacturer(ManufacturerId id, ManufacturerName name, bool isDeleted)
    {
        Id = id;
        Name = name;
        IsDeleted = isDeleted;
    }

    public ManufacturerId Id { get; }
    public ManufacturerName Name { get; private set; }
    public bool IsDeleted { get; private set; }

    public void Delete()
    {
        IsDeleted = true;
    }

    public void Modify(ManufacturerModification modification)
    {
        Name = modification.Name;
    }
}