using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Creations;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models.Factories;

public class PreparationStepFactory : IPreparationStepFactory
{
    public IPreparationStep CreateNew(PreparationStepInstruction instruction, int sortingIndex)
    {
        return new PreparationStep(PreparationStepId.New, instruction, sortingIndex);
    }

    public IPreparationStep CreateNew(PreparationStepCreation creation)
    {
        return new PreparationStep(PreparationStepId.New, creation.Instruction, creation.SortingIndex);
    }

    public IPreparationStep Create(PreparationStepId id, PreparationStepInstruction instruction, int sortingIndex)
    {
        return new PreparationStep(id, instruction, sortingIndex);
    }
}