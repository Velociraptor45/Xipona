using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.Items.Services.Creations;
using Xipona.Api.Domain.Items.Services.Queries;

namespace Xipona.Api.ApplicationServices.Items.Commands.CreateItem;

public class CreateItemCommand : ICommand<ItemReadModel>
{
    public CreateItemCommand(ItemCreation itemCreation)
    {
        ItemCreation = itemCreation;
    }

    public ItemCreation ItemCreation { get; }
}