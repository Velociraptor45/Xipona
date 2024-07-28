using System;

namespace ProjectHermes.Xipona.Api.Contracts.Recipes.Commands.ModifyRecipe
{
    /// <summary>
    /// Represents the contract for modifying a preparation step.
    /// </summary>
    public class ModifyPreparationStepContract
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="instruction"></param>
        /// <param name="sortingIndex"></param>
        public ModifyPreparationStepContract(Guid? id, string instruction, int sortingIndex)
        {
            Id = id;
            Instruction = instruction;
            SortingIndex = sortingIndex;
        }

        /// <summary>
        /// The ID of the preparation step. Null if the preparation step is new.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// The instruction of the preparation step.
        /// </summary>
        public string Instruction { get; set; }

        /// <summary>
        /// The sorting index of the preparation step.
        /// </summary>
        public int SortingIndex { get; set; }
    }
}