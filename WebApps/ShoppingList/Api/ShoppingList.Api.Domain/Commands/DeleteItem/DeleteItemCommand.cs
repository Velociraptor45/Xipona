using ShoppingList.Api.Domain.Models;

namespace ShoppingList.Api.Domain.Commands.DeleteItem
{
    public class DeleteItemCommand : ICommand<bool>
    {
        public DeleteItemCommand(StoreItemId itemId)
        {
            ItemId = itemId ?? throw new System.ArgumentNullException(nameof(itemId));
        }

        public StoreItemId ItemId { get; }
    }
}