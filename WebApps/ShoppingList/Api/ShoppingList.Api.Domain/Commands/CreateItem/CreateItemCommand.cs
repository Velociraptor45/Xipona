using ShoppingList.Api.Domain.Models;

namespace ShoppingList.Api.Domain.Commands.CreateItem
{
    public class CreateItemCommand : ICommand<bool>
    {
        public CreateItemCommand(StoreItem storeItem)
        {
            StoreItem = storeItem;
        }

        public StoreItem StoreItem { get; }
    }
}