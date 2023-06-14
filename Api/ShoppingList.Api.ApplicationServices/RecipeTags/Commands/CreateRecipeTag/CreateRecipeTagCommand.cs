using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.RecipeTags.Commands.CreateRecipeTag;

public class CreateRecipeTagCommand : ICommand<IRecipeTag>
{
    public CreateRecipeTagCommand(string name)
    {
        Name = name;
    }

    public string Name { get; }
}