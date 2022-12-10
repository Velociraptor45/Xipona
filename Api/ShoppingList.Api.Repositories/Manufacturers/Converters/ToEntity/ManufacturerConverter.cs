using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.ShoppingList.Api.Repositories.Manufacturers.Converters.ToEntity;

public class ManufacturerConverter : IToEntityConverter<IManufacturer, Entities.Manufacturer>
{
    public Entities.Manufacturer ToEntity(IManufacturer source)
    {
        return new Entities.Manufacturer()
        {
            Id = source.Id,
            Name = source.Name.Value,
            Deleted = source.IsDeleted
        };
    }
}