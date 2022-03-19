namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
public record struct Price
{
    public Price()
    {
        throw new NotSupportedException("Empty constructor is for price not supported.");
    }

    public Price(float value)
    {
        Value = value;
    }
    public float Value { get; }
}