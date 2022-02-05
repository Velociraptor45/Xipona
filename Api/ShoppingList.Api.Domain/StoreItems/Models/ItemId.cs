using System;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

public readonly record struct ItemId
{
    public ItemId()
    {
        throw new NotSupportedException("Use 'New' property to create initial value.");
    }

    public ItemId(int value)
    {
        Value = value;
    }

    public static ItemId New => new(0);

    public int Value { get; }
}