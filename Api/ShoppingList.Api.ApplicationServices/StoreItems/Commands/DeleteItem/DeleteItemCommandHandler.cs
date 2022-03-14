using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Deletions;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.DeleteItem;

public class DeleteItemCommandHandler : ICommandHandler<DeleteItemCommand, bool>
{
    private readonly Func<CancellationToken, IItemDeletionService> _itemDeletionServiceDelegate;
    private readonly ITransactionGenerator _transactionGenerator;

    public DeleteItemCommandHandler(
        Func<CancellationToken, IItemDeletionService> itemDeletionServiceDelegate,
        ITransactionGenerator transactionGenerator)
    {
        _itemDeletionServiceDelegate = itemDeletionServiceDelegate;
        _transactionGenerator = transactionGenerator;
    }

    public async Task<bool> HandleAsync(DeleteItemCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _itemDeletionServiceDelegate(cancellationToken);
        await service.DeleteAsync(command.ItemId);

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}