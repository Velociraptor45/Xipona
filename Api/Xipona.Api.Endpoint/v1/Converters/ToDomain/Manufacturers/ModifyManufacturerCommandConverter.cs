using Xipona.Api.ApplicationServices.Manufacturers.Commands.ModifyManufacturer;
using Xipona.Api.Contracts.Manufacturers.Commands;
using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Manufacturers.Models;
using Xipona.Api.Domain.Manufacturers.Services.Modifications;

namespace Xipona.Api.Endpoint.v1.Converters.ToDomain.Manufacturers;

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