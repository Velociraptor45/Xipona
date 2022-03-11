using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Commands.CreateStore;

public class CreateStoreCommandHandler : ICommandHandler<CreateStoreCommand, bool>
{
    private readonly IStoreRepository _storeRepository;
    private readonly IStoreFactory _storeFactory;
    private readonly IShoppingListFactory _shoppingListFactory;
    private readonly IShoppingListRepository _shoppingListRepository;
    private readonly ITransactionGenerator _transactionGenerator;

    public CreateStoreCommandHandler(IStoreRepository storeRepository, IStoreFactory storeFactory,
        IShoppingListFactory shoppingListFactory, IShoppingListRepository shoppingListRepository,
        ITransactionGenerator transactionGenerator)
    {
        _storeRepository = storeRepository;
        _storeFactory = storeFactory;
        _shoppingListFactory = shoppingListFactory;
        _shoppingListRepository = shoppingListRepository;
        _transactionGenerator = transactionGenerator;
    }

    public async Task<bool> HandleAsync(CreateStoreCommand command, CancellationToken cancellationToken)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        cancellationToken.ThrowIfCancellationRequested();

        IStore store = _storeFactory.CreateNew(command.StoreCreationInfo);

        using ITransaction transaction = await _transactionGenerator.GenerateAsync(cancellationToken);
        store = await _storeRepository.StoreAsync(store, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        var shoppingList = _shoppingListFactory.CreateNew(store);
        await _shoppingListRepository.StoreAsync(shoppingList, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}