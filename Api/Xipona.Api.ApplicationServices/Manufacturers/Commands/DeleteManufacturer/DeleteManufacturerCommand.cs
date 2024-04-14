using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Manufacturers.Commands.DeleteManufacturer;

public class DeleteManufacturerCommand : ICommand<bool>
{
    public DeleteManufacturerCommand(ManufacturerId manufacturerId)
    {
        ManufacturerId = manufacturerId;
    }

    public ManufacturerId ManufacturerId { get; }
}