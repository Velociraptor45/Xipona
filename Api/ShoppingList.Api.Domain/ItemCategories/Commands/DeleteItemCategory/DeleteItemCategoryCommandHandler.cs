using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Commands.DeleteItemCategory;

public class DeleteItemCategoryCommandHandler : ICommandHandler<DeleteItemCategoryCommand, bool>
{
    private readonly IItemCategoryRepository itemCategoryRepository;
    private readonly IItemRepository itemRepository;
    private readonly IShoppingListRepository shoppingListRepository;
    private readonly ITransactionGenerator transactionGenerator;

    public DeleteItemCategoryCommandHandler(IItemCategoryRepository itemCategoryRepository,
        IItemRepository itemRepository, IShoppingListRepository shoppingListRepository,
        ITransactionGenerator transactionGenerator)
    {
        this.itemCategoryRepository = itemCategoryRepository;
        this.itemRepository = itemRepository;
        this.shoppingListRepository = shoppingListRepository;
        this.transactionGenerator = transactionGenerator;
    }

    public async Task<bool> HandleAsync(DeleteItemCategoryCommand command, CancellationToken cancellationToken)
    {
        if (command is null)
        {
            throw new System.ArgumentNullException(nameof(command));
        }

        var category = await itemCategoryRepository.FindByAsync(command.ItemCategoryId, cancellationToken);
        if (category == null)
            throw new DomainException(new ItemCategoryNotFoundReason(command.ItemCategoryId));

        category.Delete();

        var storeItems = (await itemRepository.FindActiveByAsync(command.ItemCategoryId, cancellationToken))
            .ToList();

        using var transaction = await transactionGenerator.GenerateAsync(cancellationToken);
        foreach (var item in storeItems)
        {
            var lists = await shoppingListRepository.FindActiveByAsync(item.Id, cancellationToken);
            foreach (var list in lists)
            {
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
            item.Delete();
            await itemRepository.StoreAsync(item, cancellationToken);
        }
        await itemCategoryRepository.StoreAsync(category, cancellationToken);

        await transaction.CommitAsync(cancellationToken);
        return true;
    }
}