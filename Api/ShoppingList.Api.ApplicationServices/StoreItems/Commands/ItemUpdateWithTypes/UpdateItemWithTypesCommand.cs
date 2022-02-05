using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemUpdate;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.ItemUpdateWithTypes;

public class UpdateItemWithTypesCommand : ICommand<bool>
{
    public UpdateItemWithTypesCommand(ItemWithTypesUpdate itemWithTypesUpdate)
    {
        ItemWithTypesUpdate = itemWithTypesUpdate ?? throw new ArgumentNullException(nameof(itemWithTypesUpdate));
    }

    public ItemWithTypesUpdate ItemWithTypesUpdate { get; }
}