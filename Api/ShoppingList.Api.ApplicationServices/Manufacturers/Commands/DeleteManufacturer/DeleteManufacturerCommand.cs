using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Manufacturers.Commands.DeleteManufacturer;

public class DeleteManufacturerCommand : ICommand<bool>
{
    public DeleteManufacturerCommand(ManufacturerId manufacturerId)
    {
        ManufacturerId = manufacturerId;
    }

    public ManufacturerId ManufacturerId { get; }
}