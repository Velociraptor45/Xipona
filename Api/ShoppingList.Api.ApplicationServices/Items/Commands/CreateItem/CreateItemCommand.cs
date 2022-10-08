using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Commands.CreateItem;

public class CreateItemCommand : ICommand<ItemReadModel>
{
    public CreateItemCommand(ItemCreation itemCreation)
    {
        ItemCreation = itemCreation;
    }

    public ItemCreation ItemCreation { get; }
}