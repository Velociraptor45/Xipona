using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.ShoppingList.Api.Repositories.Manufacturers.Converters.ToEntity;

public class ManufacturerConverter : IToEntityConverter<IManufacturer, Entities.Manufacturer>
{
    public Entities.Manufacturer ToEntity(IManufacturer source)
    {
        return new Entities.Manufacturer()
        {
            Id = source.Id,
            Name = source.Name,
            Deleted = source.IsDeleted,
            CreatedAt = source.CreatedAt,
            RowVersion = ((AggregateRoot)source).RowVersion
        };
    }
}