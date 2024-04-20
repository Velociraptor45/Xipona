using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Common.Models;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.Xipona.Api.Repositories.Manufacturers.Converters.ToContract;

public class ManufacturerConverter : IToContractConverter<IManufacturer, Entities.Manufacturer>
{
    public Entities.Manufacturer ToContract(IManufacturer source)
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