using ShoppingList.Api.Domain.Models;

namespace ShoppingList.Api.Domain.Commands.CreateStore
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