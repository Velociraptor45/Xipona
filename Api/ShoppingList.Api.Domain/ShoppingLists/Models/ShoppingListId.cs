using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

public readonly record struct ShoppingListId
{
    public ShoppingListId()
    {
        throw new NotSupportedException("Use 'New' property to create initial value.");
    }

    public ShoppingListId(Guid value)
    {
        Value = value;
    }

    public static ShoppingListId New => new(Guid.NewGuid());

    public Guid Value { get; }

    public static implicit operator Guid(ShoppingListId shoppingListId)
    {
        return shoppingListId.Value;
    }
}