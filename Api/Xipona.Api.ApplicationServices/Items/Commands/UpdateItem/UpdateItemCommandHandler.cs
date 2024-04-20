using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Updates;
using ProjectHermes.Xipona.Api.Repositories.Common.Transactions;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Items.Commands.UpdateItem;

public class UpdateItemCommandHandler : ICommandHandler<UpdateItemCommand, bool>
{
    private readonly Func<CancellationToken, IItemUpdateService> _itemUpdateServiceDelegate;
    private readonly ITransactionGenerator _transactionGenerator;

    public UpdateItemCommandHandler(
        Func<CancellationToken, IItemUpdateService> itemUpdateServiceDelegate,
        ITransactionGenerator transactionGenerator)
    {
        _itemUpdateServiceDelegate = itemUpdateServiceDelegate;
        _transactionGenerator = transactionGenerator;
    }

    public async Task<bool> HandleAsync(UpdateItemCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _itemUpdateServiceDelegate(cancellationToken);
        await service.UpdateAsync(command.ItemUpdate);

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}