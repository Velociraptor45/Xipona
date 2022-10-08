using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.Recipes.Commands.ModifyRecipe
{
    public class ModifyPreparationStepContract
    {
        public ModifyPreparationStepContract(Guid? id, string instruction, int sortingIndex)
        {
            Id = id;
            Instruction = instruction;
            SortingIndex = sortingIndex;
        }

        public Guid? Id { get; set; }
        public string Instruction { get; set; }
        public int SortingIndex { get; set; }
    }
}