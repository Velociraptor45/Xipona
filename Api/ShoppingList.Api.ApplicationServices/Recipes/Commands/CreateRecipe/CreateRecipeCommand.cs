using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Recipes.Commands.CreateRecipe;

public class CreateRecipeCommand : ICommand<RecipeReadModel>
{
    public CreateRecipeCommand(RecipeCreation creation)
    {
        Creation = creation;
    }

    public RecipeCreation Creation { get; }
}