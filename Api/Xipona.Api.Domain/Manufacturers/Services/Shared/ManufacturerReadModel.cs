using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Shared;

public class ManufacturerReadModel
{
    public ManufacturerReadModel(ManufacturerId id, ManufacturerName name, bool isDeleted)
    {
        Id = id;
        Name = name;
        IsDeleted = isDeleted;
    }

    public ManufacturerReadModel(IManufacturer manufacturer)
    {
        Id = manufacturer.Id;
        Name = manufacturer.Name;
        IsDeleted = manufacturer.IsDeleted;
    }

    public ManufacturerId Id { get; }
    public ManufacturerName Name { get; }
    public bool IsDeleted { get; }
}