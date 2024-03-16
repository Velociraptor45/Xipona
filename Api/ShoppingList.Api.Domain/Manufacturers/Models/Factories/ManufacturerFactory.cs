using ProjectHermes.ShoppingList.Api.Core.Services;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models.Factories;

public class ManufacturerFactory : IManufacturerFactory
{
    private readonly IDateTimeService _dateTimeService;

    public ManufacturerFactory(IDateTimeService dateTimeService)
    {
        _dateTimeService = dateTimeService;
    }

    public IManufacturer Create(ManufacturerId id, ManufacturerName name, bool isDeleted, DateTimeOffset createdAt)
    {
        return new Manufacturer(id, name, isDeleted, createdAt);
    }

    public IManufacturer Create(ManufacturerName name)
    {
        return new Manufacturer(ManufacturerId.New, name, false, _dateTimeService.UtcNow);
    }
}