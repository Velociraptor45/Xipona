using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Infrastructure.Common.Transactions;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.ChangeItemQuantityOnShoppingList;

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