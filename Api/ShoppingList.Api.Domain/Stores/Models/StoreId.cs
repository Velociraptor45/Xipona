using System;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models
{
    public readonly record struct StoreId
    {
        public StoreId() : this(default)
        {
        }

        public StoreId(int value)
        {
            if (value == default)
                throw new ArgumentException($"{nameof(value)} mustn't be default ({value})");

            Value = value;
        }

        public int Value { get; }
    }
}