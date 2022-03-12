using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemCreations;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.CreateTemporaryItem;

public class CreateTemporaryItemCommand : ICommand<bool>
{
    public CreateTemporaryItemCommand(TemporaryItemCreation temporaryItemCreation)
    {
        TemporaryItemCreation = temporaryItemCreation ?? throw new ArgumentNullException(nameof(temporaryItemCreation));
    }

    public TemporaryItemCreation TemporaryItemCreation { get; }
}