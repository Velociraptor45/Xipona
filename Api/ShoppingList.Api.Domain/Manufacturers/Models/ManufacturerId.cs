namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;

public readonly record struct ManufacturerId
{
    public ManufacturerId()
    {
        throw new NotSupportedException("Use 'New' property to create initial value.");
    }

    public ManufacturerId(Guid value)
    {
        Value = value;
    }

    public static ManufacturerId New => new(Guid.NewGuid());

    public Guid Value { get; }

    public static implicit operator Guid(ManufacturerId manufacturerId)
    {
        return manufacturerId.Value;
    }

    public override string ToString()
    {
        return Value.ToString("D");
    }
}