using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.CreateTemporaryItem;

public class CreateTemporaryItemCommand : ICommand<StoreItemReadModel>
{
    public CreateTemporaryItemCommand(TemporaryItemCreation temporaryItemCreation)
    {
        TemporaryItemCreation = temporaryItemCreation ?? throw new ArgumentNullException(nameof(temporaryItemCreation));
    }

    public TemporaryItemCreation TemporaryItemCreation { get; }
}