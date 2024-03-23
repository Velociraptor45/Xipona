namespace ProjectHermes.Xipona.Api.Domain.Items.Models;

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

    public static implicit operator Guid(ItemTypeId itemTypeId)
    {
        return itemTypeId.Value;
    }

    public override string ToString()
    {
        return Value.ToString("D");
    }
}