using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Creations;

public class PreparationStepCreation
{
    public PreparationStepCreation(PreparationStepInstruction instruction, int sortingIndex)
    {
        Instruction = instruction;
        SortingIndex = sortingIndex;
    }

    public PreparationStepInstruction Instruction { get; }
    public int SortingIndex { get; }
}