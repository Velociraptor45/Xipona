namespace ShoppingList.Api.Domain.Commands.CreateItem
{
    public class CreateItemCommand : ICommand<bool>
    {
        public CreateItemCommand(ItemCreation itemCreation)
        {
            ItemCreation = itemCreation ?? throw new System.ArgumentNullException(nameof(itemCreation));
        }

        public ItemCreation ItemCreation { get; }
    }
}