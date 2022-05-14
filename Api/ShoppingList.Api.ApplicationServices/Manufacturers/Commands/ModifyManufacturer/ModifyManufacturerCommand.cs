using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Modifications;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Manufacturers.Commands.ModifyManufacturer;

public class ModifyManufacturerCommand : ICommand<bool>
{
    public ModifyManufacturerCommand(ManufacturerModification modification)
    {
        Modification = modification;
    }

    public ManufacturerModification Modification { get; }
}