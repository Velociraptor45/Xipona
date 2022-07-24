using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using PreparationStep = ProjectHermes.ShoppingList.Api.Infrastructure.Recipes.Entities.PreparationStep;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Recipes.Converters.ToContract;

public class PreparationStepConverter : IToContractConverter<(RecipeId, IPreparationStep), PreparationStep>
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