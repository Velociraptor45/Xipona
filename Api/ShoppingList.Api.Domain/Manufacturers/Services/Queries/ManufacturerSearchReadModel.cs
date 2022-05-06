using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Queries;

public class ManufacturerSearchReadModel
{
    public ManufacturerSearchReadModel(ManufacturerId id, ManufacturerName name)
    {
        Id = id;
        Name = name;
    }

    public ManufacturerId Id { get; }
    public ManufacturerName Name { get; }
}