using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.ShoppingLists.Services.AddItems;
using Xipona.Api.Repositories.Common.Transactions;

namespace Xipona.Api.ApplicationServices.ShoppingLists.Commands.AddItemsToShoppingLists;

public class AddItemsToShoppingListsCommandHandler : ICommandHandler<AddItemsToShoppingListsCommand, bool>
{
    private readonly Func<CancellationToken, IAddItemToShoppingListService> _addItemToShoppingListServiceDelegate;
    private readonly ITransactionGenerator _transactionGenerator;

    public AddItemsToShoppingListsCommandHandler(
        Func<CancellationToken, IAddItemToShoppingListService> addItemToShoppingListServiceDelegate,
        ITransactionGenerator transactionGenerator)
    {
        _addItemToShoppingListServiceDelegate = addItemToShoppingListServiceDelegate;
        _transactionGenerator = transactionGenerator;
    }

    public async Task<bool> HandleAsync(AddItemsToShoppingListsCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _addItemToShoppingListServiceDelegate(cancellationToken);
        await service.AddAsync(command.ItemToShoppingListAdditions);

        await transaction.CommitAsync(cancellationToken);
        return true;
    }
}