using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Modifications;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Commands.ModifyItem;

public class ModifyItemCommand : ICommand<bool>
{
    public ModifyItemCommand(ItemModification itemModify)
    {
        ItemModify = itemModify ?? throw new ArgumentNullException(nameof(itemModify));
    }

    public ItemModification ItemModify { get; }
}