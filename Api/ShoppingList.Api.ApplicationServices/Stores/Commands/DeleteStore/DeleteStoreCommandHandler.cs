using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Deletions;
using ProjectHermes.ShoppingList.Api.Repositories.Common.Transactions;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Stores.Commands.DeleteStore;

public class DeleteStoreCommandHandler : ICommandHandler<DeleteStoreCommand, bool>
{
    private readonly Func<CancellationToken, IStoreDeletionService> _storeDeletionServiceDelegate;
    private readonly ITransactionGenerator _transactionGenerator;

    public DeleteStoreCommandHandler(
        Func<CancellationToken, IStoreDeletionService> storeDeletionServiceDelegate,
        ITransactionGenerator transactionGenerator)
    {
        _storeDeletionServiceDelegate = storeDeletionServiceDelegate;
        _transactionGenerator = transactionGenerator;
    }

    public async Task<bool> HandleAsync(DeleteStoreCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _storeDeletionServiceDelegate(cancellationToken);
        await service.DeleteAsync(command.StoreId);

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}