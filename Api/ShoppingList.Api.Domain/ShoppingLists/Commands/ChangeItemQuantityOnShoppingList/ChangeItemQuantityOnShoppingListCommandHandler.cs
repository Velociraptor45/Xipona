using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.ChangeItemQuantityOnShoppingList;

public class ChangeItemQuantityOnShoppingListCommandHandler
    : ICommandHandler<ChangeItemQuantityOnShoppingListCommand, bool>
{
    private readonly IShoppingListRepository _shoppingListRepository;
    private readonly IItemRepository _itemRepository;

    public ChangeItemQuantityOnShoppingListCommandHandler(IShoppingListRepository shoppingListRepository,
        IItemRepository itemRepository)
    {
        this._shoppingListRepository = shoppingListRepository;
        this._itemRepository = itemRepository;
    }

    public async Task<bool> HandleAsync(ChangeItemQuantityOnShoppingListCommand command, CancellationToken cancellationToken)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        var list = await _shoppingListRepository.FindByAsync(command.ShoppingListId, cancellationToken);
        if (list == null)
            throw new DomainException(new ShoppingListNotFoundReason(command.ShoppingListId));

        ItemId itemId;
        if (command.OfflineTolerantItemId.IsActualId)
        {
            itemId = new ItemId(command.OfflineTolerantItemId.ActualId!.Value);
        }
        else
        {
            if (command.ItemTypeId != null)
                throw new DomainException(new TemporaryItemCannotHaveTypeIdReason());

            var temporaryId = new TemporaryItemId(command.OfflineTolerantItemId.OfflineId!.Value);
            var item = await _itemRepository.FindByAsync(temporaryId, cancellationToken);

            if (item == null)
                throw new DomainException(new ItemNotFoundReason(temporaryId));

            itemId = item.Id;
        }

        list.ChangeItemQuantity(itemId, command.ItemTypeId, command.Quantity);

        cancellationToken.ThrowIfCancellationRequested();

        await _shoppingListRepository.StoreAsync(list, cancellationToken);

        return true;
    }
}