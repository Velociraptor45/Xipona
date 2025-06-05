using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.Recipes.Services.Modifications;

namespace Xipona.Api.ApplicationServices.Recipes.Commands.ModifyRecipe;

public class ModifyRecipeCommand : ICommand<bool>
{
    public ModifyRecipeCommand(RecipeModification modification)
    {
        Modification = modification;
    }

    public RecipeModification Modification { get; }
}