using System;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models
{
    public readonly record struct ItemId
    {
        public ItemId() : this(default)
        {
        }

        public ItemId(int value)
        {
            if (value == default)
                throw new ArgumentException($"{nameof(value)} mustn't be default ({value})");

            Value = value;
        }

        public int Value { get; }
    }
}