using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.AddItems;
using ProjectHermes.ShoppingList.Api.Repositories.Common.Transactions;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.AddItemsToShoppingLists;

public class AddItemsToShoppingListsCommandHandler : ICommandHandler<AddItemsToShoppingListsCommand, bool>
{
    private readonly IAddItemToShoppingListService _addItemToShoppingListService;
    private readonly ITransactionGenerator _transactionGenerator;

    public AddItemsToShoppingListsCommandHandler(IAddItemToShoppingListService addItemToShoppingListService,
        ITransactionGenerator transactionGenerator)
    {
        _addItemToShoppingListService = addItemToShoppingListService;
        _transactionGenerator = transactionGenerator;
    }

    public async Task<bool> HandleAsync(AddItemsToShoppingListsCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        await _addItemToShoppingListService.AddAsync(command.ItemToShoppingListAdditions, cancellationToken);

        await transaction.CommitAsync(cancellationToken);
        return true;
    }
}