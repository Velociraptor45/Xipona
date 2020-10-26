using ShoppingList.Api.Domain.Models;

namespace ShoppingList.Api.Domain.Commands.UpdateItem
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