using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.ShoppingLists.Services.Modifications;
using Xipona.Api.Repositories.Common.Transactions;

namespace Xipona.Api.ApplicationServices.ShoppingLists.Commands.ChangeItemQuantityOnShoppingList;

public class ChangeItemQuantityOnShoppingListCommandHandler
    : ICommandHandler<ChangeItemQuantityOnShoppingListCommand, bool>
{
    private readonly Func<CancellationToken, IShoppingListModificationService> _shoppingListModificationServiceDelegate;
    private readonly ITransactionGenerator _transactionGenerator;

    public ChangeItemQuantityOnShoppingListCommandHandler(
        Func<CancellationToken, IShoppingListModificationService> shoppingListModificationServiceDelegate,
        ITransactionGenerator transactionGenerator)
    {
        _shoppingListModificationServiceDelegate = shoppingListModificationServiceDelegate;
        _transactionGenerator = transactionGenerator;
    }

    public async Task<bool> HandleAsync(ChangeItemQuantityOnShoppingListCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _shoppingListModificationServiceDelegate(cancellationToken);
        await service.ChangeItemQuantityAsync(command.ShoppingListId, command.OfflineTolerantItemId, command.ItemTypeId,
            command.Quantity);

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}