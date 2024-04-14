using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Creations;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Queries;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Items.Commands.CreateItem;

public class CreateItemCommand : ICommand<ItemReadModel>
{
    public CreateItemCommand(ItemCreation itemCreation)
    {
        ItemCreation = itemCreation;
    }

    public ItemCreation ItemCreation { get; }
}