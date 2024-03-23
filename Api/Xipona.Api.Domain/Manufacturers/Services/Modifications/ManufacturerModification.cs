using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Modifications;

public class ManufacturerModification
{
    public ManufacturerModification(ManufacturerId id, ManufacturerName name)
    {
        Id = id;
        Name = name;
    }

    public ManufacturerId Id { get; }
    public ManufacturerName Name { get; }
}