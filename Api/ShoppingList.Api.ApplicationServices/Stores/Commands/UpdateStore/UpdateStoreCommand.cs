using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Updates;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Stores.Commands.UpdateStore;

public class UpdateStoreCommand : ICommand<bool>
{
    public UpdateStoreCommand(StoreUpdate storeUpdate)
    {
        StoreUpdate = storeUpdate ?? throw new ArgumentNullException(nameof(storeUpdate));
    }

    public StoreUpdate StoreUpdate { get; }
}