using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.DeleteItem;

public class DeleteItemCommandHandler : ICommandHandler<DeleteItemCommand, bool>
{
    private readonly IItemRepository _itemRepository;
    private readonly IShoppingListRepository _shoppingListRepository;
    private readonly ITransactionGenerator _transactionGenerator;

    public DeleteItemCommandHandler(IItemRepository itemRepository, IShoppingListRepository shoppingListRepository,
        ITransactionGenerator transactionGenerator)
    {
        _itemRepository = itemRepository;
        _shoppingListRepository = shoppingListRepository;
        _transactionGenerator = transactionGenerator;
    }

    public async Task<bool> HandleAsync(DeleteItemCommand command, CancellationToken cancellationToken)
    {
        if (command is null)
            throw new ArgumentNullException(nameof(command));

        var item = await _itemRepository.FindByAsync(command.ItemId, cancellationToken);
        if (item == null)
            throw new DomainException(new ItemNotFoundReason(command.ItemId));

        item.Delete();
        var listsWithItem = (await _shoppingListRepository.FindActiveByAsync(item.Id, cancellationToken)).ToList();

        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);
        await _itemRepository.StoreAsync(item, cancellationToken);

        foreach (var list in listsWithItem)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (item.HasItemTypes)
            {
                foreach (var type in item.ItemTypes)
                {
                    list.RemoveItem(item.Id, type.Id);
                }
            }
            else
            {
                list.RemoveItem(item.Id);
            }

            await _shoppingListRepository.StoreAsync(list, cancellationToken);
        }

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}