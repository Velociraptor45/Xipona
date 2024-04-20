using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Manufacturers.Queries.ManufacturerById;

public class ManufacturerByIdQuery : IQuery<IManufacturer>
{
    public ManufacturerByIdQuery(ManufacturerId manufacturerId)
    {
        ManufacturerId = manufacturerId;
    }

    public ManufacturerId ManufacturerId { get; }
}