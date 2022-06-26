using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Modifications;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.ItemCategories.Commands.ModifyItemCategory;

public class ModifyItemCategoryCommand : ICommand<bool>
{
    public ModifyItemCategoryCommand(ItemCategoryModification modification)
    {
        Modification = modification;
    }

    public ItemCategoryModification Modification { get; }
}