using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.ItemCategories.Commands.CreateItemCategory;

public class CreateItemCategoryCommand : ICommand<IItemCategory>
{
    public CreateItemCategoryCommand(ItemCategoryName name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public ItemCategoryName Name { get; }
}