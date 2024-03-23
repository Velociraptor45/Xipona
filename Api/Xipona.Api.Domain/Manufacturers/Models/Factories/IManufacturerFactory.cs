namespace ProjectHermes.Xipona.Api.Domain.Manufacturers.Models.Factories;

public interface IManufacturerFactory
{
    IManufacturer Create(ManufacturerId id, ManufacturerName name, bool isDeleted, DateTimeOffset createdAt);

    IManufacturer Create(ManufacturerName name);
}