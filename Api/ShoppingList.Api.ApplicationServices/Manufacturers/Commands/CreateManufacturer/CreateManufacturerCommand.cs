using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Manufacturers.Commands.CreateManufacturer;

public class CreateManufacturerCommand : ICommand<bool>
{
    public CreateManufacturerCommand(ManufacturerName name)
    {
        Name = name;
    }

    public ManufacturerName Name { get; }
}