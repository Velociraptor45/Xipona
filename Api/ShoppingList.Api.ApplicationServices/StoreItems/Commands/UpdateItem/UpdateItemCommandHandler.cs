using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemUpdates;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.UpdateItem;

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
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _itemUpdateServiceDelegate(cancellationToken);
        await service.Update(command.ItemUpdate);

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}