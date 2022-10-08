using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.AddItems;
using ProjectHermes.ShoppingList.Api.Infrastructure.Common.Transactions;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.AddItemWithTypeToShoppingList;

public class AddItemWithTypeToShoppingListCommandHandler
    : ICommandHandler<AddItemWithTypeToShoppingListCommand, bool>
{
    private readonly IAddItemToShoppingListService _addItemToShoppingListService;
    private readonly ITransactionGenerator _transactionGenerator;

    public AddItemWithTypeToShoppingListCommandHandler(IAddItemToShoppingListService addItemToShoppingListService,
        ITransactionGenerator transactionGenerator)
    {
        _addItemToShoppingListService = addItemToShoppingListService;
        _transactionGenerator = transactionGenerator;
    }

    public async Task<bool> HandleAsync(AddItemWithTypeToShoppingListCommand command,
        CancellationToken cancellationToken)
    {
        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        await _addItemToShoppingListService.AddItemWithTypeToShoppingListAsync(command.ShoppingListId, command.ItemId,
            command.ItemTypeId, command.SectionId, command.Quantity, cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}