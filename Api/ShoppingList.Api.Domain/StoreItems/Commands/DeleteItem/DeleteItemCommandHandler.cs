using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.DeleteItem;

public class DeleteItemCommandHandler : ICommandHandler<DeleteItemCommand, bool>
{
    private readonly IItemRepository itemRepository;
    private readonly IShoppingListRepository shoppingListRepository;
    private readonly ITransactionGenerator transactionGenerator;

    public DeleteItemCommandHandler(IItemRepository itemRepository, IShoppingListRepository shoppingListRepository,
        ITransactionGenerator transactionGenerator)
    {
        this.itemRepository = itemRepository;
        this.shoppingListRepository = shoppingListRepository;
        this.transactionGenerator = transactionGenerator;
    }

    public async Task<bool> HandleAsync(DeleteItemCommand command, CancellationToken cancellationToken)
    {
        if (command is null)
            throw new ArgumentNullException(nameof(command));

        var item = await itemRepository.FindByAsync(command.ItemId, cancellationToken);
        if (item == null)
            throw new DomainException(new ItemNotFoundReason(command.ItemId));

        item.Delete();
        var listsWithItem = (await shoppingListRepository.FindActiveByAsync(item.Id, cancellationToken)).ToList();

        using var transaction = await transactionGenerator.GenerateAsync(cancellationToken);
        await itemRepository.StoreAsync(item, cancellationToken);

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

            await shoppingListRepository.StoreAsync(list, cancellationToken);
        }

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}