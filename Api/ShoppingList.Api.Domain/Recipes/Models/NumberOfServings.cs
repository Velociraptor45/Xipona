namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
public readonly record struct NumberOfServings
{
    public NumberOfServings()
    {
        throw new NotSupportedException("Empty constructor for NumberOfServings not supported");
    }

    public NumberOfServings(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static implicit operator int(NumberOfServings numberOfServings)
    {
        return numberOfServings.Value;
    }
}