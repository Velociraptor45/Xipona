using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.ShoppingListModifications;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.RemoveItemFromShoppingList;

public class RemoveItemFromShoppingListCommandHandler :
    ICommandHandler<RemoveItemFromShoppingListCommand, bool>
{
    private readonly Func<CancellationToken, IShoppingListModificationService> _shoppingListModificationServiceDelegate;
    private readonly ITransactionGenerator _transactionGenerator;

    public RemoveItemFromShoppingListCommandHandler(
        Func<CancellationToken, IShoppingListModificationService> shoppingListModificationServiceDelegate,
        ITransactionGenerator transactionGenerator)
    {
        _shoppingListModificationServiceDelegate = shoppingListModificationServiceDelegate;
        _transactionGenerator = transactionGenerator;
    }

    public async Task<bool> HandleAsync(RemoveItemFromShoppingListCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _shoppingListModificationServiceDelegate(cancellationToken);
        await service.RemoveItemAsync(command.ShoppingListId, command.OfflineTolerantItemId, command.ItemTypeId);

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}