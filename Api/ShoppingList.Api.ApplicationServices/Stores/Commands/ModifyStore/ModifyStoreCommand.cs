using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Modifications;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Stores.Commands.ModifyStore;

public class ModifyStoreCommand : ICommand<bool>
{
    public ModifyStoreCommand(StoreModification storeUpdate)
    {
        StoreUpdate = storeUpdate;
    }

    public StoreModification StoreUpdate { get; }
}