﻿namespace ProjectHermes.Xipona.Api.Domain.Items.Models;

public readonly record struct ItemId
{
    public ItemId()
    {
        throw new NotSupportedException("Use 'New' property to create initial value.");
    }

    public ItemId(Guid value)
    {
        Value = value;
    }

    public static ItemId New => new(Guid.NewGuid());

    public Guid Value { get; }

    public static implicit operator Guid(ItemId itemId)
    {
        return itemId.Value;
    }

    public override string ToString()
    {
        return Value.ToString("D");
    }
}