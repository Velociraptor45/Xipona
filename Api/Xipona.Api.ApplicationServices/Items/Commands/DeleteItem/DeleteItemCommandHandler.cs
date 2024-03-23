using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Deletions;
using ProjectHermes.Xipona.Api.Repositories.Common.Transactions;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Items.Commands.DeleteItem;

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
        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _itemDeletionServiceDelegate(cancellationToken);
        await service.DeleteAsync(command.ItemId);

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}