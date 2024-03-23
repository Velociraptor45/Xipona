using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using PreparationStep = ProjectHermes.Xipona.Api.Repositories.Recipes.Entities.PreparationStep;

namespace ProjectHermes.Xipona.Api.Repositories.Recipes.Converters.ToContract;

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