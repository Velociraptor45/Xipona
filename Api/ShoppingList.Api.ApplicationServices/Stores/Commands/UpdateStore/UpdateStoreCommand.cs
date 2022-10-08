using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Updates;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Stores.Commands.UpdateStore;

public class UpdateStoreCommand : ICommand<bool>
{
    public UpdateStoreCommand(StoreUpdate storeUpdate)
    {
        StoreUpdate = storeUpdate;
    }

    public StoreUpdate StoreUpdate { get; }
}