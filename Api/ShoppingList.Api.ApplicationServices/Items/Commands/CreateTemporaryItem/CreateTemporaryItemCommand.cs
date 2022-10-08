using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Commands.CreateTemporaryItem;

public class CreateTemporaryItemCommand : ICommand<ItemReadModel>
{
    public CreateTemporaryItemCommand(TemporaryItemCreation temporaryItemCreation)
    {
        TemporaryItemCreation = temporaryItemCreation;
    }

    public TemporaryItemCreation TemporaryItemCreation { get; }
}