using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.DeleteItem
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