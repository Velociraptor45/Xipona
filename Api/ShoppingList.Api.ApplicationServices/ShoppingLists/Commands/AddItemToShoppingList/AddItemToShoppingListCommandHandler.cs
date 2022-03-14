using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.AddItems;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.AddItemToShoppingList;

public class AddItemToShoppingListCommandHandler : ICommandHandler<AddItemToShoppingListCommand, bool>
{
    private readonly IAddItemToShoppingListService _addItemToShoppingListService;

    public AddItemToShoppingListCommandHandler(IAddItemToShoppingListService addItemToShoppingListService)
    {
        _addItemToShoppingListService = addItemToShoppingListService;
    }

    public async Task<bool> HandleAsync(AddItemToShoppingListCommand command, CancellationToken cancellationToken)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        await _addItemToShoppingListService.AddAsync(command.ShoppingListId, command.ItemId, command.SectionId,
            command.Quantity, cancellationToken);

        return true;
    }
}