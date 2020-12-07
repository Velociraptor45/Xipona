using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Commands.UpdateStore
{
    public class UpdateStoreCommand : ICommand<bool>
    {
        public UpdateStoreCommand(StoreUpdate storeUpdate)
        {
            StoreUpdate = storeUpdate ?? throw new System.ArgumentNullException(nameof(storeUpdate));
        }

        public StoreUpdate StoreUpdate { get; }
    }
}