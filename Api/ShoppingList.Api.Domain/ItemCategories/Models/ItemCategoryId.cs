using System;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models
{
    public readonly record struct ItemCategoryId
    {
        public ItemCategoryId() : this(default)
        {
        }

        public ItemCategoryId(int value)
        {
            if (value == default)
                throw new ArgumentException($"{nameof(value)} mustn't be default ({value})");

            Value = value;
        }

        public int Value { get; }
    }
}