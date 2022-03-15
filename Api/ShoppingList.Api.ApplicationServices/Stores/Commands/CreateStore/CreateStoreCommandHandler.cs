using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Creations;
using ProjectHermes.ShoppingList.Api.Infrastructure.Common.Transactions;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Stores.Commands.CreateStore;

public class CreateStoreCommandHandler : ICommandHandler<CreateStoreCommand, bool>
{
    private readonly Func<CancellationToken, IStoreCreationService> _storeCreationServiceDelegate;
    private readonly ITransactionGenerator _transactionGenerator;

    public CreateStoreCommandHandler(Func<CancellationToken, IStoreCreationService> storeCreationServiceDelegate,
        ITransactionGenerator transactionGenerator)
    {
        _storeCreationServiceDelegate = storeCreationServiceDelegate;
        _transactionGenerator = transactionGenerator;
    }

    public async Task<bool> HandleAsync(CreateStoreCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        using ITransaction transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _storeCreationServiceDelegate(cancellationToken);
        await service.CreateAsync(command.StoreCreation);

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}