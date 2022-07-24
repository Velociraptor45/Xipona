namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
public readonly record struct PreparationStepId
{
    public PreparationStepId()
    {
        throw new NotSupportedException("Use 'New' property to create initial value.");
    }

    public PreparationStepId(Guid value)
    {
        Value = value;
    }

    public static PreparationStepId New => new(Guid.NewGuid());

    public Guid Value { get; }
}