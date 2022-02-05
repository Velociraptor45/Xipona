using System;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;

public readonly record struct ManufacturerId
{
    public ManufacturerId()
    {
        throw new NotSupportedException("Use 'New' property to create initial value.");
    }

    public ManufacturerId(int value)
    {
        Value = value;
    }

    public static ManufacturerId New => new(0);

    public int Value { get; }
}