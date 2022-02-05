using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.PutItemInBasket;

public class PutItemInBasketCommandHandler : ICommandHandler<PutItemInBasketCommand, bool>
{
    private readonly IShoppingListRepository _shoppingListRepository;
    private readonly IItemRepository _itemRepository;

    public PutItemInBasketCommandHandler(IShoppingListRepository shoppingListRepository,
        IItemRepository itemRepository)
    {
        _shoppingListRepository = shoppingListRepository;
        _itemRepository = itemRepository;
    }

    public async Task<bool> HandleAsync(PutItemInBasketCommand command, CancellationToken cancellationToken)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        var shoppingList = await _shoppingListRepository.FindByAsync(command.ShoppingListId, cancellationToken);
        if (shoppingList == null)
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
            IStoreItem? item = await _itemRepository.FindByAsync(temporaryId, cancellationToken);

            if (item == null)
                throw new DomainException(new ItemNotFoundReason(temporaryId));

            itemId = item.Id;
        }

        shoppingList.PutItemInBasket(itemId, command.ItemTypeId);

        cancellationToken.ThrowIfCancellationRequested();

        await _shoppingListRepository.StoreAsync(shoppingList, cancellationToken);

        return true;
    }
}