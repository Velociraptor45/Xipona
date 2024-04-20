using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Modifications;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Manufacturers.Commands.ModifyManufacturer;

public class ModifyManufacturerCommand : ICommand<bool>
{
    public ModifyManufacturerCommand(ManufacturerModification modification)
    {
        Modification = modification;
    }

    public ManufacturerModification Modification { get; }
}