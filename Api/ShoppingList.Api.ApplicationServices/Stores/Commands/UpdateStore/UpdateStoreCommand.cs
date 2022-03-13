using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.StoreUpdate;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Stores.Commands.UpdateStore;

public class UpdateStoreCommand : ICommand<bool>
{
    public UpdateStoreCommand(StoreUpdate storeUpdate)
    {
        StoreUpdate = storeUpdate ?? throw new ArgumentNullException(nameof(storeUpdate));
    }

    public StoreUpdate StoreUpdate { get; }
}