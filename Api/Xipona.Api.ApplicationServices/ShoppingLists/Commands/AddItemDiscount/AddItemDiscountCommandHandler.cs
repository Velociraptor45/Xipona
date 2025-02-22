using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Modifications;
using ProjectHermes.Xipona.Api.Repositories.Common.Transactions;

namespace ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Commands.AddItemDiscount;

public class AddItemDiscountCommandHandler : ICommandHandler<AddItemDiscountCommand, bool>
{
    private readonly ITransactionGenerator _transactionGenerator;
    private readonly Func<CancellationToken, IShoppingListModificationService> _modificationServiceDelegate;

    public AddItemDiscountCommandHandler(ITransactionGenerator transactionGenerator,
        Func<CancellationToken, IShoppingListModificationService> modificationServiceDelegate)
    {
        _transactionGenerator = transactionGenerator;
        _modificationServiceDelegate = modificationServiceDelegate;
    }

    public async Task<bool> HandleAsync(AddItemDiscountCommand command, CancellationToken cancellationToken)
    {

        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var modificationService = _modificationServiceDelegate(cancellationToken);
        await modificationService.AddDiscountAsync(command.ShoppingListId, command.Discount);

        await transaction.CommitAsync(cancellationToken);
        return true;
    }
}