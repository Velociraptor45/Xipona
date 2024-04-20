using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.AddItems;
using ProjectHermes.Xipona.Api.Repositories.Common.Transactions;

namespace ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Commands.AddItemToShoppingList;

public class AddItemToShoppingListCommandHandler : ICommandHandler<AddItemToShoppingListCommand, bool>
{
    private readonly Func<CancellationToken, IAddItemToShoppingListService> _addItemToShoppingListServiceDelegate;
    private readonly ITransactionGenerator _transactionGenerator;

    public AddItemToShoppingListCommandHandler(
        Func<CancellationToken, IAddItemToShoppingListService> addItemToShoppingListServiceDelegate,
        ITransactionGenerator transactionGenerator)
    {
        _addItemToShoppingListServiceDelegate = addItemToShoppingListServiceDelegate;
        _transactionGenerator = transactionGenerator;
    }

    public async Task<bool> HandleAsync(AddItemToShoppingListCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _addItemToShoppingListServiceDelegate(cancellationToken);
        await service.AddAsync(command.ShoppingListId, command.ItemId, command.SectionId, command.Quantity);

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}