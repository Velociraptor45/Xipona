using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Creations;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Recipes.Commands.CreateRecipe;

public class CreateRecipeCommand : ICommand<IRecipe>
{
    public CreateRecipeCommand(RecipeCreation creation)
    {
        Creation = creation;
    }

    public RecipeCreation Creation { get; }
}