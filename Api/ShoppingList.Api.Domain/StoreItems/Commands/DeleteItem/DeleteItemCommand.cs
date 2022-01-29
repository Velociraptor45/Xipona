using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.DeleteItem
{
    public class DeleteItemCommand : ICommand<bool>
    {
        public DeleteItemCommand(ItemId itemId)
        {
            ItemId = itemId;
        }

        public ItemId ItemId { get; }
    }
}