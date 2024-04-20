using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Manufacturers.Commands.CreateManufacturer;

public class CreateManufacturerCommand : ICommand<IManufacturer>
{
    public CreateManufacturerCommand(ManufacturerName name)
    {
        Name = name;
    }

    public ManufacturerName Name { get; }
}