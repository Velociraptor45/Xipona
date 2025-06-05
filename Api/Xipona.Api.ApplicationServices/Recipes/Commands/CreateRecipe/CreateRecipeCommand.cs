using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.Recipes.Services.Creations;
using Xipona.Api.Domain.Recipes.Services.Queries;

namespace Xipona.Api.ApplicationServices.Recipes.Commands.CreateRecipe;

public class CreateRecipeCommand : ICommand<RecipeReadModel>
{
    public CreateRecipeCommand(RecipeCreation creation)
    {
        Creation = creation;
    }

    public RecipeCreation Creation { get; }
}