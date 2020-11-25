namespace ShoppingList.Api.Domain.Commands.CreateTemporaryItem
{
    public class CreateTemporaryItemCommand : ICommand<bool>
    {
        public CreateTemporaryItemCommand(TemporaryItemCreation temporaryItemCreation)
        {
            TemporaryItemCreation = temporaryItemCreation ?? throw new System.ArgumentNullException(nameof(temporaryItemCreation));
        }

        public TemporaryItemCreation TemporaryItemCreation { get; }
    }
}