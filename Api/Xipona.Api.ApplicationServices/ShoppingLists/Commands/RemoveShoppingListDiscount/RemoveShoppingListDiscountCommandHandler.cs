using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.ShoppingLists.Services.Modifications;
using Xipona.Api.Repositories.Common.Transactions;

namespace Xipona.Api.ApplicationServices.ShoppingLists.Commands.RemoveShoppingListDiscount;
public class RemoveShoppingListDiscountCommandHandler : ICommandHandler<RemoveShoppingListDiscountCommand, bool>
{
    private readonly ITransactionGenerator _transactionGenerator;
    private readonly Func<CancellationToken, IShoppingListModificationService> _modificationServiceDelegate;

    public RemoveShoppingListDiscountCommandHandler(ITransactionGenerator transactionGenerator,
        Func<CancellationToken, IShoppingListModificationService> modificationServiceDelegate)
    {
        _transactionGenerator = transactionGenerator;
        _modificationServiceDelegate = modificationServiceDelegate;
    }

    public async Task<bool> HandleAsync(RemoveShoppingListDiscountCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var modificationService = _modificationServiceDelegate(cancellationToken);
        await modificationService.RemoveDiscountAsync(command.ShoppingListId, command.ListDiscountId);

        await transaction.CommitAsync(cancellationToken);
        return true;
    }
}
