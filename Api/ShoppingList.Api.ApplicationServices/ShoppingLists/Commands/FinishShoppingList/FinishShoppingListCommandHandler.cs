using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Modifications;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.FinishShoppingList;

public class FinishShoppingListCommandHandler : ICommandHandler<FinishShoppingListCommand, bool>
{
    private readonly Func<CancellationToken, IShoppingListModificationService> _shoppingListModificationServiceDelegate;
    private readonly ITransactionGenerator _transactionGenerator;

    public FinishShoppingListCommandHandler(
        Func<CancellationToken, IShoppingListModificationService> shoppingListModificationServiceDelegate,
        ITransactionGenerator transactionGenerator)
    {
        _shoppingListModificationServiceDelegate = shoppingListModificationServiceDelegate;
        _transactionGenerator = transactionGenerator;
    }

    public async Task<bool> HandleAsync(FinishShoppingListCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _shoppingListModificationServiceDelegate(cancellationToken);
        await service.FinishAsync(command.ShoppingListId, command.CompletionDate);

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}