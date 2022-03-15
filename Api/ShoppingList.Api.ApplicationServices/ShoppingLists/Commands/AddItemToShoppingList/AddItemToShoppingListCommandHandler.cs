using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.AddItems;
using ProjectHermes.ShoppingList.Api.Infrastructure.Common.Transactions;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.AddItemToShoppingList;

public class AddItemToShoppingListCommandHandler : ICommandHandler<AddItemToShoppingListCommand, bool>
{
    private readonly IAddItemToShoppingListService _addItemToShoppingListService;
    private readonly ITransactionGenerator _transactionGenerator;

    public AddItemToShoppingListCommandHandler(IAddItemToShoppingListService addItemToShoppingListService,
        ITransactionGenerator transactionGenerator)
    {
        _addItemToShoppingListService = addItemToShoppingListService;
        _transactionGenerator = transactionGenerator;
    }

    public async Task<bool> HandleAsync(AddItemToShoppingListCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        await _addItemToShoppingListService.AddAsync(command.ShoppingListId, command.ItemId, command.SectionId,
            command.Quantity, cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}