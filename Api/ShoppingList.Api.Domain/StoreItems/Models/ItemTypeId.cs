using System;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

public readonly record struct ItemTypeId
{
    public ItemTypeId()
    {
        throw new NotSupportedException("Use 'New' property to create initial value.");
    }

    public ItemTypeId(int value)
    {
        Value = value;
    }

    public static ItemTypeId New => new(0);

    public int Value { get; }
}