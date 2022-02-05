using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Commands.CreateItemCategory;

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