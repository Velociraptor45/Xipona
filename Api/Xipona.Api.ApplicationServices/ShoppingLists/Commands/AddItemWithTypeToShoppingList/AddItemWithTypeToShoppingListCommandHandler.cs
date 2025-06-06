using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.ShoppingLists.Services.AddItems;
using Xipona.Api.Repositories.Common.Transactions;

namespace Xipona.Api.ApplicationServices.ShoppingLists.Commands.AddItemWithTypeToShoppingList;

public class AddItemWithTypeToShoppingListCommandHandler
    : ICommandHandler<AddItemWithTypeToShoppingListCommand, bool>
{
    private readonly Func<CancellationToken, IAddItemToShoppingListService> _addItemToShoppingListServiceDelegate;
    private readonly ITransactionGenerator _transactionGenerator;

    public AddItemWithTypeToShoppingListCommandHandler(
        Func<CancellationToken, IAddItemToShoppingListService> addItemToShoppingListServiceDelegate,
        ITransactionGenerator transactionGenerator)
    {
        _addItemToShoppingListServiceDelegate = addItemToShoppingListServiceDelegate;
        _transactionGenerator = transactionGenerator;
    }

    public async Task<bool> HandleAsync(AddItemWithTypeToShoppingListCommand command,
        CancellationToken cancellationToken)
    {
        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _addItemToShoppingListServiceDelegate(cancellationToken);
        await service.AddItemWithTypeAsync(command.ShoppingListId, command.ItemId,
            command.ItemTypeId, command.SectionId, command.Quantity);

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}