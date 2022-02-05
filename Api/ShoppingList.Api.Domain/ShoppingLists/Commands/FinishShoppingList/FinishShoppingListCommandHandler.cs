using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.FinishShoppingList;

public class FinishShoppingListCommandHandler : ICommandHandler<FinishShoppingListCommand, bool>
{
    private readonly IShoppingListRepository _shoppingListRepository;
    private readonly ITransactionGenerator _transactionGenerator;

    public FinishShoppingListCommandHandler(IShoppingListRepository shoppingListRepository,
        ITransactionGenerator transactionGenerator)
    {
        _shoppingListRepository = shoppingListRepository;
        _transactionGenerator = transactionGenerator;
    }

    public async Task<bool> HandleAsync(FinishShoppingListCommand command, CancellationToken cancellationToken)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        var shoppingList = await _shoppingListRepository.FindByAsync(command.ShoppingListId, cancellationToken);
        if (shoppingList == null)
            throw new DomainException(new ShoppingListNotFoundReason(command.ShoppingListId));

        cancellationToken.ThrowIfCancellationRequested();

        IShoppingList nextShoppingList = shoppingList.Finish(command.CompletionDate);

        cancellationToken.ThrowIfCancellationRequested();

        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);
        await _shoppingListRepository.StoreAsync(shoppingList, cancellationToken);
        await _shoppingListRepository.StoreAsync(nextShoppingList, cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}