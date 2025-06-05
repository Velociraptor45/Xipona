using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.Manufacturers.Models;

namespace Xipona.Api.ApplicationServices.Manufacturers.Commands.CreateManufacturer;

public class CreateManufacturerCommand : ICommand<IManufacturer>
{
    public CreateManufacturerCommand(ManufacturerName name)
    {
        Name = name;
    }

    public ManufacturerName Name { get; }
}