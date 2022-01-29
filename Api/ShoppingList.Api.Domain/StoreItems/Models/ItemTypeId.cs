using System;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models
{
    public readonly record struct ItemTypeId
    {
        public ItemTypeId() : this(default)
        {
        }

        public ItemTypeId(int value)
        {
            if (value == default)
                throw new ArgumentException($"{nameof(value)} mustn't be default ({value})");

            Value = value;
        }

        public int Value { get; }
    }
}