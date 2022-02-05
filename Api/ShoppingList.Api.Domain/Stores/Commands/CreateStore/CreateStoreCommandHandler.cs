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
    private readonly IStoreRepository storeRepository;
    private readonly IStoreFactory storeFactory;
    private readonly IShoppingListFactory shoppingListFactory;
    private readonly IShoppingListRepository shoppingListRepository;
    private readonly ITransactionGenerator transactionGenerator;

    public CreateStoreCommandHandler(IStoreRepository storeRepository, IStoreFactory storeFactory,
        IShoppingListFactory shoppingListFactory, IShoppingListRepository shoppingListRepository,
        ITransactionGenerator transactionGenerator)
    {
        this.storeRepository = storeRepository;
        this.storeFactory = storeFactory;
        this.shoppingListFactory = shoppingListFactory;
        this.shoppingListRepository = shoppingListRepository;
        this.transactionGenerator = transactionGenerator;
    }

    public async Task<bool> HandleAsync(CreateStoreCommand command, CancellationToken cancellationToken)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        cancellationToken.ThrowIfCancellationRequested();

        IStore store = storeFactory.Create(command.StoreCreationInfo.Id, command.StoreCreationInfo.Name, false,
            command.StoreCreationInfo.Sections);

        using ITransaction transaction = await transactionGenerator.GenerateAsync(cancellationToken);
        store = await storeRepository.StoreAsync(store, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        var shoppingList = shoppingListFactory.CreateNew(store);
        await shoppingListRepository.StoreAsync(shoppingList, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}