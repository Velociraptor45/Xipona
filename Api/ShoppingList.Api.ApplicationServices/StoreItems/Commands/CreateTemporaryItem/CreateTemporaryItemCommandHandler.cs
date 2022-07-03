using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;
using ProjectHermes.ShoppingList.Api.Infrastructure.Common.Transactions;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.CreateTemporaryItem;

public class CreateTemporaryItemCommandHandler : ICommandHandler<CreateTemporaryItemCommand, StoreItemReadModel>
{
    private readonly Func<CancellationToken, IItemCreationService> _itemCreationServiceDelegate;
    private readonly ITransactionGenerator _transactionGenerator;

    public CreateTemporaryItemCommandHandler(
        Func<CancellationToken, IItemCreationService> itemCreationServiceDelegate,
        ITransactionGenerator transactionGenerator)
    {
        _itemCreationServiceDelegate = itemCreationServiceDelegate;
        _transactionGenerator = transactionGenerator;
    }

    public async Task<StoreItemReadModel> HandleAsync(CreateTemporaryItemCommand command, CancellationToken cancellationToken)
    {
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _itemCreationServiceDelegate(cancellationToken);
        var result = await service.CreateTemporaryAsync(command.TemporaryItemCreation);

        await transaction.CommitAsync(cancellationToken);

        return result;
    }
}