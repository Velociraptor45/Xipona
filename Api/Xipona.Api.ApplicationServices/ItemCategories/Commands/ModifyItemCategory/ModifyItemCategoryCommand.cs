using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.ItemCategories.Services.Modifications;

namespace Xipona.Api.ApplicationServices.ItemCategories.Commands.ModifyItemCategory;

public class ModifyItemCategoryCommand : ICommand<bool>
{
    public ModifyItemCategoryCommand(ItemCategoryModification modification)
    {
        Modification = modification;
    }

    public ItemCategoryModification Modification { get; }
}