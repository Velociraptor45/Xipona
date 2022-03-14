using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Creations;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.CreateTemporaryItem;

public class CreateTemporaryItemCommandHandler : ICommandHandler<CreateTemporaryItemCommand, bool>
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

    public async Task<bool> HandleAsync(CreateTemporaryItemCommand command, CancellationToken cancellationToken)
    {
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _itemCreationServiceDelegate(cancellationToken);
        await service.CreateTemporaryAsync(command.TemporaryItemCreation);

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}