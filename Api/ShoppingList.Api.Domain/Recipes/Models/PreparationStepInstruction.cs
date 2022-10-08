namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

public class PreparationStepInstruction
{
    public PreparationStepInstruction(string value)
    {
        Value = value;
    }

    public string Value { get; }
}