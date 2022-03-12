using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.ChangeItem;

public class ModifyItemCommand : ICommand<bool>
{
    public ModifyItemCommand(ItemModification itemModify)
    {
        ItemModify = itemModify ?? throw new ArgumentNullException(nameof(itemModify));
    }

    public ItemModification ItemModify { get; }
}