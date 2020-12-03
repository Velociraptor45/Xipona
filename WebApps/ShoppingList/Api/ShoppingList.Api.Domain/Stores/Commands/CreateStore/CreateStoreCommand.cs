using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ShoppingList.Api.Domain.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Commands.CreateStore
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