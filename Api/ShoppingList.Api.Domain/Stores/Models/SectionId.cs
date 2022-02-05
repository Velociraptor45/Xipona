using System;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

public readonly record struct SectionId
{
    public SectionId()
    {
        throw new NotSupportedException("Use 'New' property to create initial value.");
    }

    public SectionId(int value)
    {
        Value = value;
    }

    public static SectionId New => new(0);

    public int Value { get; }
}