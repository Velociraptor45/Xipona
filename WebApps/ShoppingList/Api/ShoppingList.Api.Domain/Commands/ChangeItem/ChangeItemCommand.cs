namespace ShoppingList.Api.Domain.Commands.ChangeItem
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