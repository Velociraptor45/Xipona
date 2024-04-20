using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Creations;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Recipes.Commands.CreateRecipe;

public class CreateRecipeCommand : ICommand<RecipeReadModel>
{
    public CreateRecipeCommand(RecipeCreation creation)
    {
        Creation = creation;
    }

    public RecipeCreation Creation { get; }
}