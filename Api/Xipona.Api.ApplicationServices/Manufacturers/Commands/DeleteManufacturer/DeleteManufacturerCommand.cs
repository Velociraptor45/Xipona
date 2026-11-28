using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.Manufacturers.Models;

namespace Xipona.Api.ApplicationServices.Manufacturers.Commands.DeleteManufacturer;

public class DeleteManufacturerCommand : ICommand<bool>
{
    public DeleteManufacturerCommand(ManufacturerId manufacturerId)
    {
        ManufacturerId = manufacturerId;
    }

    public ManufacturerId ManufacturerId { get; }
}