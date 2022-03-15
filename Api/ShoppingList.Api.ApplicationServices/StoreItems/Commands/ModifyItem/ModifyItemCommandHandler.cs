using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Infrastructure.Common.Transactions;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.ModifyItem;

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
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _itemModificationServiceDelegate(cancellationToken);
        await service.Modify(command.ItemModify);

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}