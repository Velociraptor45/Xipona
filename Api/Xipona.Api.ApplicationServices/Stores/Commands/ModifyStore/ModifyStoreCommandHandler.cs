using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.Stores.Services.Modifications;
using ProjectHermes.Xipona.Api.Repositories.Common.Transactions;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Stores.Commands.ModifyStore;

public class ModifyStoreCommandHandler : ICommandHandler<ModifyStoreCommand, bool>
{
    private readonly Func<CancellationToken, IStoreModificationService> _storeUpdateServiceDelegate;
    private readonly ITransactionGenerator _transactionGenerator;

    public ModifyStoreCommandHandler(Func<CancellationToken, IStoreModificationService> storeUpdateServiceDelegate,
        ITransactionGenerator transactionGenerator)
    {
        _storeUpdateServiceDelegate = storeUpdateServiceDelegate;
        _transactionGenerator = transactionGenerator;
    }

    public async Task<bool> HandleAsync(ModifyStoreCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _storeUpdateServiceDelegate(cancellationToken);
        await service.ModifyAsync(command.StoreUpdate);

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}