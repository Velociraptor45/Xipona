using Xipona.Api.ApplicationServices.Manufacturers.Commands.DeleteManufacturer;
using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Manufacturers.Models;

namespace Xipona.Api.Endpoint.v1.Converters.ToDomain.Manufacturers;

public class DeleteManufacturerCommandConverter : IToDomainConverter<Guid, DeleteManufacturerCommand>
{
    public DeleteManufacturerCommand ToDomain(Guid source)
    {
        return new DeleteManufacturerCommand(new ManufacturerId(source));
    }
}