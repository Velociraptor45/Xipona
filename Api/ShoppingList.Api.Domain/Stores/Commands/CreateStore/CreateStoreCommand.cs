using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Commands.CreateStore;

public class CreateStoreCommand : ICommand<bool>
{
    public CreateStoreCommand(StoreCreationInfo storeCreationInfo)
    {
        StoreCreationInfo = storeCreationInfo;
    }

    public StoreCreationInfo StoreCreationInfo { get; }
}