using Xipona.Api.ApplicationServices.Manufacturers.Commands.CreateManufacturer;
using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Manufacturers.Models;

namespace Xipona.Api.Endpoint.v1.Converters.ToDomain.Manufacturers;

public class CreateManufacturerCommandConverter : IToDomainConverter<string, CreateManufacturerCommand>
{
    public CreateManufacturerCommand ToDomain(string source)
    {
        return new CreateManufacturerCommand(new ManufacturerName(source));
    }
}