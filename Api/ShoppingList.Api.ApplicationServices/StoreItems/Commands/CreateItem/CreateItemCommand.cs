using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemCreations;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.CreateItem;

public class CreateItemCommand : ICommand<bool>
{
    public CreateItemCommand(ItemCreation itemCreation)
    {
        ItemCreation = itemCreation ?? throw new ArgumentNullException(nameof(itemCreation));
    }

    public ItemCreation ItemCreation { get; }
}