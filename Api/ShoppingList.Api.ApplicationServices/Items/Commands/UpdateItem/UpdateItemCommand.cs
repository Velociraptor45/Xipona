using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Updates;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Commands.UpdateItem;

public class UpdateItemCommand : ICommand<bool>
{
    public UpdateItemCommand(ItemUpdate itemUpdate)
    {
        ItemUpdate = itemUpdate ?? throw new ArgumentNullException(nameof(itemUpdate));
    }

    public ItemUpdate ItemUpdate { get; }
}