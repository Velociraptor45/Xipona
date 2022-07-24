namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

public class PreparationStep : IPreparationStep
{
    public PreparationStep(PreparationStepId id, PreparationStepInstruction instruction, int sortingIndex)
    {
        Id = id;
        Instruction = instruction;
        SortingIndex = sortingIndex;
    }

    public PreparationStepId Id { get; }
    public PreparationStepInstruction Instruction { get; }
    public int SortingIndex { get; }
}