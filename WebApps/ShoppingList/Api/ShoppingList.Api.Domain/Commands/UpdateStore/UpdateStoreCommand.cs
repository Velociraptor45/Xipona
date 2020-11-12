namespace ShoppingList.Api.Domain.Commands.UpdateStore
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