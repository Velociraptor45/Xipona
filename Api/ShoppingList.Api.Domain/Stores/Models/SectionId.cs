﻿namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

public readonly record struct SectionId
{
    public SectionId()
    {
        throw new NotSupportedException("Use 'New' property to create initial value.");
    }

    public SectionId(Guid value)
    {
        Value = value;
    }

    public static SectionId New => new(Guid.NewGuid());

    public Guid Value { get; }
}