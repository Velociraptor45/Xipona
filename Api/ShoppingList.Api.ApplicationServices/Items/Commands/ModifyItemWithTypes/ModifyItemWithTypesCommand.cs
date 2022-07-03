using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Modifications;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Commands.ModifyItemWithTypes;

public class ModifyItemWithTypesCommand : ICommand<bool>
{
    public ModifyItemWithTypesCommand(ItemWithTypesModification itemWithTypesModification)
    {
        ItemWithTypesModification = itemWithTypesModification ??
                                    throw new ArgumentNullException(nameof(itemWithTypesModification));
    }

    public ItemWithTypesModification ItemWithTypesModification { get; }
}