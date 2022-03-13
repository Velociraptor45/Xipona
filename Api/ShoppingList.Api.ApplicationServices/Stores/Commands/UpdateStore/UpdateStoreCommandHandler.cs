using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.StoreUpdate;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Stores.Commands.UpdateStore;

public class UpdateStoreCommandHandler : ICommandHandler<UpdateStoreCommand, bool>
{
    private readonly Func<CancellationToken, IStoreUpdateService> _storeUpdateServiceDelegate;
    private readonly ITransactionGenerator _transactionGenerator;

    public UpdateStoreCommandHandler(Func<CancellationToken, IStoreUpdateService> storeUpdateServiceDelegate,
        ITransactionGenerator transactionGenerator)
    {
        _storeUpdateServiceDelegate = storeUpdateServiceDelegate;
        _transactionGenerator = transactionGenerator;
    }

    public async Task<bool> HandleAsync(UpdateStoreCommand command, CancellationToken cancellationToken)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _storeUpdateServiceDelegate(cancellationToken);
        await service.UpdateAsync(command.StoreUpdate);

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}