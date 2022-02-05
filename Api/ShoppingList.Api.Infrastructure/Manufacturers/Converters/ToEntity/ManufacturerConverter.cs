using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Manufacturers.Converters.ToEntity;

public class ManufacturerConverter : IToEntityConverter<IManufacturer, Entities.Manufacturer>
{
    public Entities.Manufacturer ToEntity(IManufacturer source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return new Entities.Manufacturer()
        {
            Id = source.Id.Value,
            Name = source.Name,
            Deleted = source.IsDeleted
        };
    }
}