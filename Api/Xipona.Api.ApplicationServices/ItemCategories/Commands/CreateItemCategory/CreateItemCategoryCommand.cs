using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.ItemCategories.Models;

namespace Xipona.Api.ApplicationServices.ItemCategories.Commands.CreateItemCategory;

public class CreateItemCategoryCommand : ICommand<IItemCategory>
{
    public CreateItemCategoryCommand(ItemCategoryName name)
    {
        Name = name;
    }

    public ItemCategoryName Name { get; }
}