using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Items.Services.Updates;
using Xipona.Api.Repositories.Common.Transactions;

namespace Xipona.Api.ApplicationServices.Items.Commands.MergeItems;

public record MergeItemsCommand(MergedItem Item) : ICommand<ItemId>;

public class MergeItemsCommandHandler : ICommandHandler<MergeItemsCommand, ItemId>
{
    private readonly ITransactionGenerator _transactionGenerator;
    private readonly Func<CancellationToken, IItemMergeService> _serviceDelegate;

    public MergeItemsCommandHandler(ITransactionGenerator transactionGenerator,
        Func<CancellationToken, IItemMergeService> serviceDelegate)
    {
        _transactionGenerator = transactionGenerator;
        _serviceDelegate = serviceDelegate;
    }

    public async Task<ItemId> HandleAsync(MergeItemsCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _serviceDelegate(cancellationToken);
        var itemId = await service.MergeAsync(command.Item);

        await transaction.CommitAsync(cancellationToken);

        return itemId;
    }
}
