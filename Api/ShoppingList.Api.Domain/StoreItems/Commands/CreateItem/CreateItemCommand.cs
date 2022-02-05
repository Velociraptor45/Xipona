using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateItem;

public class CreateItemCommand : ICommand<bool>
{
    public CreateItemCommand(ItemCreation itemCreation)
    {
        ItemCreation = itemCreation ?? throw new ArgumentNullException(nameof(itemCreation));
    }

    public ItemCreation ItemCreation { get; }
}