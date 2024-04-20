using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.Stores.Services.Deletions;
using ProjectHermes.Xipona.Api.Repositories.Common.Transactions;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Stores.Commands.DeleteStore;

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