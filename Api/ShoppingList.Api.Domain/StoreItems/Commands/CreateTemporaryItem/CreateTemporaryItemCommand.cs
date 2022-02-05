using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateTemporaryItem;

public class CreateTemporaryItemCommand : ICommand<bool>
{
    public CreateTemporaryItemCommand(TemporaryItemCreation temporaryItemCreation)
    {
        TemporaryItemCreation = temporaryItemCreation ?? throw new ArgumentNullException(nameof(temporaryItemCreation));
    }

    public TemporaryItemCreation TemporaryItemCreation { get; }
}