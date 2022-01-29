using System;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models
{
    public readonly record struct ManufacturerId
    {
        public ManufacturerId() : this(default)
        {
        }

        public ManufacturerId(int value)
        {
            if (value == default)
                throw new ArgumentException($"{nameof(value)} mustn't be default ({value})");

            Value = value;
        }

        public int Value { get; }
    }
}