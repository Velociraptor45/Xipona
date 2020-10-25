using ShoppingList.Domain.Models;

namespace ShoppingList.Domain.Commands.UpdateItem
{
    public class UpdateItemCommand : ICommand<bool>
    {
        public UpdateItemCommand(StoreItem storeItem)
        {
            StoreItem = storeItem;
        }

        public StoreItem StoreItem { get; }
    }
}