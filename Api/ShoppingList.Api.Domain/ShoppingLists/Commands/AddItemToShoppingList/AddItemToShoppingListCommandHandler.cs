using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.AddItemToShoppingList;

public class AddItemToShoppingListCommandHandler : ICommandHandler<AddItemToShoppingListCommand, bool>
{
    private readonly IShoppingListRepository _shoppingListRepository;
    private readonly IAddItemToShoppingListService _addItemToShoppingListService;

    public AddItemToShoppingListCommandHandler(IShoppingListRepository shoppingListRepository,
        IAddItemToShoppingListService addItemToShoppingListService)
    {
        _shoppingListRepository = shoppingListRepository;
        _addItemToShoppingListService = addItemToShoppingListService;
    }

    public async Task<bool> HandleAsync(AddItemToShoppingListCommand command, CancellationToken cancellationToken)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        var list = await _shoppingListRepository.FindByAsync(command.ShoppingListId, cancellationToken);
        if (list == null)
            throw new DomainException(new ShoppingListNotFoundReason(command.ShoppingListId));

        cancellationToken.ThrowIfCancellationRequested();

        if (command.ItemId.IsActualId)
        {
            var actualId = new ItemId(command.ItemId.ActualId!.Value);
            await _addItemToShoppingListService.AddItemToShoppingList(list, actualId, command.SectionId, command.Quantity,
                cancellationToken);
        }
        else
        {
            var temporaryId = new TemporaryItemId(command.ItemId.OfflineId!.Value);
            await _addItemToShoppingListService.AddItemToShoppingList(list, temporaryId, command.SectionId,
                command.Quantity, cancellationToken);
        }

        cancellationToken.ThrowIfCancellationRequested();

        await _shoppingListRepository.StoreAsync(list, cancellationToken);

        return true;
    }
}