using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.Recipes.Models;

public class PreparationStep
{
    public PreparationStep(Guid id, string instruction, int sortingIndex)
    {
        Id = id;
        Instruction = instruction;
        SortingIndex = sortingIndex;
    }

    public Guid Id { get; }
    public string Instruction { get; }
    public int SortingIndex { get; }
}