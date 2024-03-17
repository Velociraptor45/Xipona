using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Modifications;
using ProjectHermes.Xipona.Api.Repositories.Common.Transactions;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Items.Commands.ModifyItem;

public class ModifyItemCommandHandler : ICommandHandler<ModifyItemCommand, bool>
{
    private readonly Func<CancellationToken, IItemModificationService> _itemModificationServiceDelegate;
    private readonly ITransactionGenerator _transactionGenerator;

    public ModifyItemCommandHandler(
        Func<CancellationToken, IItemModificationService> itemModificationServiceDelegate,
        ITransactionGenerator transactionGenerator)
    {
        _itemModificationServiceDelegate = itemModificationServiceDelegate;
        _transactionGenerator = transactionGenerator;
    }

    public async Task<bool> HandleAsync(ModifyItemCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _itemModificationServiceDelegate(cancellationToken);
        await service.Modify(command.ItemModify);

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}