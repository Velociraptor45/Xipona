using ProjectHermes.Xipona.Api.ApplicationServices.Manufacturers.Commands.ModifyManufacturer;
using ProjectHermes.Xipona.Api.Contracts.Manufacturers.Commands;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Modifications;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToDomain.Manufacturers;

public class ModifyManufacturerCommandConverter :
    IToDomainConverter<ModifyManufacturerContract, ModifyManufacturerCommand>
{
    public ModifyManufacturerCommand ToDomain(ModifyManufacturerContract source)
    {
        return new ModifyManufacturerCommand(
            new ManufacturerModification(
                new ManufacturerId(source.ManufacturerId),
                new ManufacturerName(source.Name)));
    }
}