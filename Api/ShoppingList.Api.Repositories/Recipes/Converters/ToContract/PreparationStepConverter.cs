using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using PreparationStep = ProjectHermes.ShoppingList.Api.Repositories.Recipes.Entities.PreparationStep;

namespace ProjectHermes.ShoppingList.Api.Repositories.Recipes.Converters.ToContract;

public class PreparationStepConverter : IToContractConverter<(RecipeId, IPreparationStep), Entities.PreparationStep>
{
    public PreparationStep ToContract((RecipeId, IPreparationStep) source)
    {
        (RecipeId recipeId, IPreparationStep? preparationStep) = source;

        return new PreparationStep
        {
            Id = preparationStep.Id.Value,
            RecipeId = recipeId.Value,
            Instruction = preparationStep.Instruction.Value,
            SortingIndex = preparationStep.SortingIndex
        };
    }
}