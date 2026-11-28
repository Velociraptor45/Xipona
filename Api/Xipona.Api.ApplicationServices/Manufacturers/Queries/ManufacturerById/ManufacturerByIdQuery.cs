using Xipona.Api.ApplicationServices.Common.Queries;
using Xipona.Api.Domain.Manufacturers.Models;

namespace Xipona.Api.ApplicationServices.Manufacturers.Queries.ManufacturerById;

public class ManufacturerByIdQuery : IQuery<IManufacturer>
{
    public ManufacturerByIdQuery(ManufacturerId manufacturerId)
    {
        ManufacturerId = manufacturerId;
    }

    public ManufacturerId ManufacturerId { get; }
}