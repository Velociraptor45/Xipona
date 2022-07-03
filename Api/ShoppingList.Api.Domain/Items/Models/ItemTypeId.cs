namespace ProjectHermes.ShoppingList.Api.Domain.Items.Models;

public readonly record struct ItemTypeId
{
    public ItemTypeId()
    {
        throw new NotSupportedException("Use 'New' property to create initial value.");
    }

    public ItemTypeId(Guid value)
    {
        Value = value;
    }

    public static ItemTypeId New => new(Guid.NewGuid());

    public Guid Value { get; }
}