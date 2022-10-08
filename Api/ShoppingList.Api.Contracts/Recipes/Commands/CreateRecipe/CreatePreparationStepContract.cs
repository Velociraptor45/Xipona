namespace ProjectHermes.ShoppingList.Api.Contracts.Recipes.Commands.CreateRecipe
{
    public class CreatePreparationStepContract
    {
        public CreatePreparationStepContract(string instruction, int sortingIndex)
        {
            Instruction = instruction;
            SortingIndex = sortingIndex;
        }

        public string Instruction { get; set; }
        public int SortingIndex { get; set; }
    }
}