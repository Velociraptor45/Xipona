using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.ChangeItem
{
    public class ChangeItemCommand : ICommand<bool>
    {
        public ChangeItemCommand(ItemChange itemChange)
        {
            ItemChange = itemChange ?? throw new System.ArgumentNullException(nameof(itemChange));
        }

        public ItemChange ItemChange { get; }
    }
}