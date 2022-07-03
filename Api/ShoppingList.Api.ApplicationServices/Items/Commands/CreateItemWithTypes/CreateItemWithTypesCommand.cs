using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Commands.CreateItemWithTypes;

public class CreateItemWithTypesCommand : ICommand<StoreItemReadModel>
{
    public CreateItemWithTypesCommand(IItem item)
    {
        Item = item;
    }

    public IItem Item { get; }
}