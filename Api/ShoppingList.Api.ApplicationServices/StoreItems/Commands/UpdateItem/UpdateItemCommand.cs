using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Updates;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.UpdateItem;

public class UpdateItemCommand : ICommand<bool>
{
    public UpdateItemCommand(ItemUpdate itemUpdate)
    {
        ItemUpdate = itemUpdate ?? throw new ArgumentNullException(nameof(itemUpdate));
    }

    public ItemUpdate ItemUpdate { get; }
}