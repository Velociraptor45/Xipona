namespace Xipona.Api.Domain.ShoppingLists.Models;
public readonly record struct ListDiscountId
{
    public ListDiscountId()
    {
        throw new NotSupportedException("Use 'New' property to create initial value.");
    }

    public ListDiscountId(Guid value)
    {
        Value = value;
    }

    public static ListDiscountId New => new(Guid.CreateVersion7());

    public Guid Value { get; }

    public static implicit operator Guid(ListDiscountId shoppingListId)
    {
        return shoppingListId.Value;
    }

    public override string ToString()
    {
        return Value.ToString("D");
    }
}