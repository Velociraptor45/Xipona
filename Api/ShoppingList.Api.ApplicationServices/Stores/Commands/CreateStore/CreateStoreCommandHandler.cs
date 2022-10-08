using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Creations;
using ProjectHermes.ShoppingList.Api.Infrastructure.Common.Transactions;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Stores.Commands.CreateStore;

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