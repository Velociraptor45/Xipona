using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.ShoppingLists.Services.Modifications;
using Xipona.Api.Repositories.Common.Transactions;

namespace Xipona.Api.ApplicationServices.ShoppingLists.Commands.AddTemporaryItemToShoppingList;

public class AddTemporaryItemToShoppingListCommandHandler :
    ICommandHandler<AddTemporaryItemToShoppingListCommand, TemporaryShoppingListItemReadModel>
{
    private readonly Func<CancellationToken, IShoppingListModificationService> _modificationServiceDelegate;
    private readonly ITransactionGenerator _transactionGenerator;

    public AddTemporaryItemToShoppingListCommandHandler(
        Func<CancellationToken, IShoppingListModificationService> modificationServiceDelegate,
        ITransactionGenerator transactionGenerator)
    {
        _modificationServiceDelegate = modificationServiceDelegate;
        _transactionGenerator = transactionGenerator;
    }

    public async Task<TemporaryShoppingListItemReadModel> HandleAsync(AddTemporaryItemToShoppingListCommand command,
        CancellationToken cancellationToken)
    {
        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _modificationServiceDelegate(cancellationToken);
        var shoppingListItem = await service.AddTemporaryItemAsync(command.ShoppingListId, command.ItemName,
            command.QuantityType, command.Quantity, command.Price, command.SectionId, command.TemporaryItemId);

        await transaction.CommitAsync(cancellationToken);

        return shoppingListItem;
    }
}