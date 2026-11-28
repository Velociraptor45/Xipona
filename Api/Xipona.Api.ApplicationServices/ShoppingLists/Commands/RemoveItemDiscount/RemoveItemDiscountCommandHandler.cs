using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.ShoppingLists.Services.Modifications;
using Xipona.Api.Repositories.Common.Transactions;

namespace Xipona.Api.ApplicationServices.ShoppingLists.Commands.RemoveItemDiscount;

public class RemoveItemDiscountCommandHandler : ICommandHandler<RemoveItemDiscountCommand, bool>
{
    private readonly ITransactionGenerator _transactionGenerator;
    private readonly Func<CancellationToken, IShoppingListModificationService> _modificationServiceDelegate;

    public RemoveItemDiscountCommandHandler(ITransactionGenerator transactionGenerator,
        Func<CancellationToken, IShoppingListModificationService> modificationServiceDelegate)
    {
        _transactionGenerator = transactionGenerator;
        _modificationServiceDelegate = modificationServiceDelegate;
    }

    public async Task<bool> HandleAsync(RemoveItemDiscountCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var modificationService = _modificationServiceDelegate(cancellationToken);
        await modificationService.RemoveDiscountAsync(command.ShoppingListId, command.ItemId, command.ItemTypeId);

        await transaction.CommitAsync(cancellationToken);
        return true;
    }
}
