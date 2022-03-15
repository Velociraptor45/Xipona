using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Modifications;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.ModifyItem;

public class ModifyItemCommand : ICommand<bool>
{
    public ModifyItemCommand(ItemModification itemModify)
    {
        ItemModify = itemModify ?? throw new ArgumentNullException(nameof(itemModify));
    }

    public ItemModification ItemModify { get; }
}