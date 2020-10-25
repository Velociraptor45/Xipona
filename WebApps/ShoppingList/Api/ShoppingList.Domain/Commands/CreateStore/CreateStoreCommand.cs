using ShoppingList.Domain.Models;

namespace ShoppingList.Domain.Commands.CreateStore
{
    public class CreateStoreCommand : ICommand<bool>
    {
        public CreateStoreCommand(Store store)
        {
            Store = store;
        }

        public Store Store { get; }
    }
}