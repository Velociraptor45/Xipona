using ShoppingList.Domain.Models;

namespace ShoppingList.Domain.Commands.UpdateStore
{
    public class UpdateStoreCommand : ICommand<bool>
    {
        public UpdateStoreCommand(Store store)
        {
            Store = store;
        }

        public Store Store { get; }
    }
}