using System;

namespace ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.Get
{
    /// <summary>
    /// Represents the contract for a preparation step.
    /// </summary>
    public class PreparationStepContract
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="instruction"></param>
        /// <param name="sortingIndex"></param>
        public PreparationStepContract(Guid id, string instruction, int sortingIndex)
        {
            Id = id;
            Instruction = instruction;
            SortingIndex = sortingIndex;
        }

        /// <summary>
        /// The ID of the preparation step.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// The instruction of the preparation step.
        /// </summary>
        public string Instruction { get; }

        /// <summary>
        /// The sorting index of the preparation step.
        /// </summary>
        public int SortingIndex { get; }
    }
}