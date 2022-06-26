using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.CreateItem;

public class CreateItemCommand : ICommand<StoreItemReadModel>
{
    public CreateItemCommand(ItemCreation itemCreation)
    {
        ItemCreation = itemCreation ?? throw new ArgumentNullException(nameof(itemCreation));
    }

    public ItemCreation ItemCreation { get; }
}