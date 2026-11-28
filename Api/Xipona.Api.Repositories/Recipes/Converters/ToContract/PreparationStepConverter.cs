using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Recipes.Models;
using PreparationStep = Xipona.Api.Repositories.Recipes.Entities.PreparationStep;

namespace Xipona.Api.Repositories.Recipes.Converters.ToContract;

public class PreparationStepConverter : IToContractConverter<(RecipeId, IPreparationStep), Entities.PreparationStep>
{
    public PreparationStep ToContract((RecipeId, IPreparationStep) source)
    {
        (RecipeId recipeId, IPreparationStep? preparationStep) = source;

        return new PreparationStep
        {
            Id = preparationStep.Id,
            RecipeId = recipeId,
            Instruction = preparationStep.Instruction.Value,
            SortingIndex = preparationStep.SortingIndex
        };
    }
}