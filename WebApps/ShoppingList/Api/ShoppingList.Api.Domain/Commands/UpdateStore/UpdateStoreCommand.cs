using ShoppingList.Api.Domain.Models;

namespace ShoppingList.Api.Domain.Commands.UpdateStore
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