namespace ShoppingList.Api.Domain.Commands.UpdateItem
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