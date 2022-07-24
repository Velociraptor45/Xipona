namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

public interface IPreparationStep
{
    PreparationStepId Id { get; }
    PreparationStepInstruction Instruction { get; }
    int SortingIndex { get; }
}