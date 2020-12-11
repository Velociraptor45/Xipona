using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.ChangeItem
{
    public class ModifyItemCommand : ICommand<bool>
    {
        public ModifyItemCommand(ItemModify itemModify)
        {
            ItemModify = itemModify ?? throw new System.ArgumentNullException(nameof(itemModify));
        }

        public ItemModify ItemModify { get; }
    }
}