using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;

namespace ProjectHermes.Xipona.Api.ApplicationServices.ItemCategories.Commands.CreateItemCategory;

public class CreateItemCategoryCommand : ICommand<IItemCategory>
{
    public CreateItemCategoryCommand(ItemCategoryName name)
    {
        Name = name;
    }

    public ItemCategoryName Name { get; }
}