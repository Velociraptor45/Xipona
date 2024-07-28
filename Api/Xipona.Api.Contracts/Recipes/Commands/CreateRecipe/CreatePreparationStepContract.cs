namespace ProjectHermes.Xipona.Api.Contracts.Recipes.Commands.CreateRecipe
{
    /// <summary>
    /// Represents the contract for creating a preparation step.
    /// </summary>
    public class CreatePreparationStepContract
    {
        /// <summary>
        /// </summary>
        /// <param name="instruction"></param>
        /// <param name="sortingIndex"></param>
        public CreatePreparationStepContract(string instruction, int sortingIndex)
        {
            Instruction = instruction;
            SortingIndex = sortingIndex;
        }

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