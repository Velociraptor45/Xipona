using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Repositories.Common.Transactions;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.AddTemporaryItemToShoppingList;

public class AddTemporaryItemToShoppingListCommandHandler : ICommandHandler<AddTemporaryItemToShoppingListCommand, bool>
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

    public async Task<bool> HandleAsync(AddTemporaryItemToShoppingListCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _modificationServiceDelegate(cancellationToken);
        await service.AddTemporaryItemAsync(command.ShoppingListId, command.ItemName, command.QuantityType,
            command.Quantity, command.Price, command.SectionId, command.TemporaryItemId);

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}