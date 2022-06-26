using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries;
using ProjectHermes.ShoppingList.Api.Infrastructure.Common.Transactions;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.CreateItem;

public class CreateItemCommandHandler : ICommandHandler<CreateItemCommand, StoreItemReadModel>
{
    private readonly Func<CancellationToken, IItemCreationService> _itemCreationServiceDelegate;
    private readonly ITransactionGenerator _transactionGenerator;

    public CreateItemCommandHandler(Func<CancellationToken, IItemCreationService> itemCreationServiceDelegate,
        ITransactionGenerator transactionGenerator)
    {
        _itemCreationServiceDelegate = itemCreationServiceDelegate;
        _transactionGenerator = transactionGenerator;
    }

    public async Task<StoreItemReadModel> HandleAsync(CreateItemCommand command, CancellationToken cancellationToken)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var itemCreationService = _itemCreationServiceDelegate(cancellationToken);
        var result = await itemCreationService.CreateAsync(command.ItemCreation);

        await transaction.CommitAsync(cancellationToken);

        return result;
    }
}