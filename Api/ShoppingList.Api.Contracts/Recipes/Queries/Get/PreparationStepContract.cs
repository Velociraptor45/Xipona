using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.Get
{
    public class PreparationStepContract
    {
        public PreparationStepContract(Guid id, string instruction, int sortingIndex)
        {
            Id = id;
            Instruction = instruction;
            SortingIndex = sortingIndex;
        }

        public Guid Id { get; }
        public string Instruction { get; }
        public int SortingIndex { get; }
    }
}