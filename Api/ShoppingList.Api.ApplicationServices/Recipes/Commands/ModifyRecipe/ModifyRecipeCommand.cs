using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Recipes.Commands.ModifyRecipe;

public class ModifyRecipeCommand : ICommand<bool>
{
    public ModifyRecipeCommand(RecipeModification modification)
    {
        Modification = modification;
    }

    public RecipeModification Modification { get; }
}