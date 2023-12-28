namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
public readonly record struct IngredientId
{
    public IngredientId()
    {
        throw new NotSupportedException("Use 'New' property to create initial value.");
    }

    public IngredientId(Guid value)
    {
        Value = value;
    }

    public static IngredientId New => new(Guid.NewGuid());

    public Guid Value { get; }

    public static implicit operator Guid(IngredientId ingredientId)
    {
        return ingredientId.Value;
    }

    public override string ToString()
    {
        return Value.ToString("D");
    }
}