using ProjectHermes.ShoppingList.Frontend.Models.Shared;
using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.Recipes.Models;

public class PreparationStep : ISortableItem
{
    public PreparationStep(Guid id, string instruction, int sortingIndex)
    {
        Id = id;
        Name = instruction;
        SortingIndex = sortingIndex;
    }

    public Guid Id { get; }
    public string Name { get; set; }
    public int SortingIndex { get; private set; }

    public void SetSortingIndex(int index)
    {
        SortingIndex = index;
    }
}