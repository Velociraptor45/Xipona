using ProjectHermes.ShoppingList.Api.ApplicationServices.Manufacturers.Commands.ModifyManufacturer;
using ProjectHermes.ShoppingList.Api.Contracts.Manufacturers.Commands;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Modifications;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.Manufacturers;

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