using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.Stores.Models;
using Xipona.Api.Domain.Stores.Services.Creations;
using Xipona.Api.Repositories.Common.Transactions;

namespace Xipona.Api.ApplicationServices.Stores.Commands.CreateStore;

public class CreateStoreCommandHandler : ICommandHandler<CreateStoreCommand, IStore>
{
    private readonly Func<CancellationToken, IStoreCreationService> _storeCreationServiceDelegate;
    private readonly ITransactionGenerator _transactionGenerator;

    public CreateStoreCommandHandler(Func<CancellationToken, IStoreCreationService> storeCreationServiceDelegate,
        ITransactionGenerator transactionGenerator)
    {
        _storeCreationServiceDelegate = storeCreationServiceDelegate;
        _transactionGenerator = transactionGenerator;
    }

    public async Task<IStore> HandleAsync(CreateStoreCommand command, CancellationToken cancellationToken)
    {
        using ITransaction transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _storeCreationServiceDelegate(cancellationToken);
        var result = await service.CreateAsync(command.StoreCreation);

        await transaction.CommitAsync(cancellationToken);

        return result;
    }
}