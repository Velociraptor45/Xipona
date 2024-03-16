using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models.Factories;

namespace ProjectHermes.ShoppingList.Api.Repositories.Manufacturers.Converters.ToDomain;

public class ManufacturerConverter : IToDomainConverter<Entities.Manufacturer, IManufacturer>
{
    private readonly IManufacturerFactory _manufacturerFactory;

    public ManufacturerConverter(IManufacturerFactory manufacturerFactory)
    {
        _manufacturerFactory = manufacturerFactory;
    }

    public IManufacturer ToDomain(Entities.Manufacturer source)
    {
        var manufacturer = (AggregateRoot)_manufacturerFactory.Create(
            new ManufacturerId(source.Id),
            new ManufacturerName(source.Name),
            source.Deleted,
            source.CreatedAt);

        manufacturer.EnrichWithRowVersion(source.RowVersion);
        return (manufacturer as IManufacturer)!;
    }
}