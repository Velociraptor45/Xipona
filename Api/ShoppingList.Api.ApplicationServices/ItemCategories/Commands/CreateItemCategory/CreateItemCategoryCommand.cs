using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.ItemCategories.Commands.CreateItemCategory;

public class CreateItemCategoryCommand : ICommand<bool>
{
    public CreateItemCategoryCommand(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace", nameof(name));
        }

        Name = name;
    }

    public string Name { get; }
}