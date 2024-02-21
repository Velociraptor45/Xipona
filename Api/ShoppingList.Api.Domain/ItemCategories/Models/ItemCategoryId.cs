namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;

public readonly record struct ItemCategoryId
{
    public ItemCategoryId()
    {
        throw new NotSupportedException("Use 'New' property to create initial value.");
    }

    public ItemCategoryId(Guid value)
    {
        Value = value;
    }

    public static ItemCategoryId New => new(Guid.NewGuid());

    public Guid Value { get; }

    public static implicit operator Guid(ItemCategoryId itemCategoryId)
    {
        return itemCategoryId.Value;
    }

    public override string ToString()
    {
        return Value.ToString("D");
    }
}