using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;

public class PreparationStepModification
{
    public PreparationStepModification(PreparationStepId? id, PreparationStepInstruction instruction, int sortingIndex)
    {
        Id = id;
        Instruction = instruction;
        SortingIndex = sortingIndex;
    }

    public PreparationStepId? Id { get; }
    public PreparationStepInstruction Instruction { get; }
    public int SortingIndex { get; }
}