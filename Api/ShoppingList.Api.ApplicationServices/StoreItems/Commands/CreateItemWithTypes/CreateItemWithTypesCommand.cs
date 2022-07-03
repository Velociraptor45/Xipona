using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.CreateItemWithTypes;

public class CreateItemWithTypesCommand : ICommand<StoreItemReadModel>
{
    public CreateItemWithTypesCommand(IItem item)
    {
        Item = item;
    }

    public IItem Item { get; }
}