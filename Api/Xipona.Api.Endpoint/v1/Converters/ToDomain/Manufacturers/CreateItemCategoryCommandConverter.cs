using ProjectHermes.Xipona.Api.ApplicationServices.Manufacturers.Commands.CreateManufacturer;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToDomain.Manufacturers;

public class CreateManufacturerCommandConverter : IToDomainConverter<string, CreateManufacturerCommand>
{
    public CreateManufacturerCommand ToDomain(string source)
    {
        return new CreateManufacturerCommand(new ManufacturerName(source));
    }
}