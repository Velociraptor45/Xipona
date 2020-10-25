using ShoppingList.Domain.Models;

namespace ShoppingList.Domain.Commands.CreateItem
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