namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

public readonly record struct StoreId
{
    public StoreId()
    {
        throw new NotSupportedException("Use 'New' property to create initial value.");
    }

    public StoreId(Guid value)
    {
        Value = value;
    }

    public static StoreId New => new(Guid.NewGuid());

    public Guid Value { get; }

    public static implicit operator Guid(StoreId storeId)
    {
        return storeId.Value;
    }

    public override string ToString()
    {
        return Value.ToString("D");
    }
}