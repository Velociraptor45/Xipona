using System;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models
{
    public readonly record struct ShoppingListId
    {
        public ShoppingListId() : this(default)
        {
        }

        public ShoppingListId(int value)
        {
            if (value == default)
                throw new ArgumentException($"{nameof(value)} mustn't be default ({value})");

            Value = value;
        }

        public int Value { get; }
    }
}