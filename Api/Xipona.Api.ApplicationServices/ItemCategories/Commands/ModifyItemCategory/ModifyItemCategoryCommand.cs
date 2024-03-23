using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Modifications;

namespace ProjectHermes.Xipona.Api.ApplicationServices.ItemCategories.Commands.ModifyItemCategory;

public class ModifyItemCategoryCommand : ICommand<bool>
{
    public ModifyItemCategoryCommand(ItemCategoryModification modification)
    {
        Modification = modification;
    }

    public ItemCategoryModification Modification { get; }
}