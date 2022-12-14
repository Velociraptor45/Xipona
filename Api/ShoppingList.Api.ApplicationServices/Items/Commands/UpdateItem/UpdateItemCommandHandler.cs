using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Updates;
using ProjectHermes.ShoppingList.Api.Repositories.Common.Transactions;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Commands.UpdateItem;

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