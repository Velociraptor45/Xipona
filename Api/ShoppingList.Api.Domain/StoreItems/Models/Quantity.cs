namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

public record struct Quantity
{
    public Quantity()
    {
        throw new NotSupportedException("Empty constructor for Quantity not supported.");
    }

    public Quantity(float value)
    {
        Value = value;
    }

    public float Value { get; }
}