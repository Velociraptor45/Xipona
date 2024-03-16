using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Modifications;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;

public interface IManufacturer
{
    ManufacturerId Id { get; }
    ManufacturerName Name { get; }
    bool IsDeleted { get; }
    DateTimeOffset CreatedAt { get; }

    void Delete();

    void Modify(ManufacturerModification modification);
}