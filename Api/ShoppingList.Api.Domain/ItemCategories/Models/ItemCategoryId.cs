using System;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;

public readonly record struct ItemCategoryId
{
    public ItemCategoryId()
    {
        throw new NotSupportedException("Use 'New' property to create initial value.");
    }

    public ItemCategoryId(int value)
    {
        Value = value;
    }

    public static ItemCategoryId New => new(0);

    public int Value { get; }
}