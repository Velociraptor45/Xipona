using ProjectHermes.Xipona.Api.ApplicationServices.Manufacturers.Commands.DeleteManufacturer;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToDomain.Manufacturers;

public class DeleteManufacturerCommandConverter : IToDomainConverter<Guid, DeleteManufacturerCommand>
{
    public DeleteManufacturerCommand ToDomain(Guid source)
    {
        return new DeleteManufacturerCommand(new ManufacturerId(source));
    }
}