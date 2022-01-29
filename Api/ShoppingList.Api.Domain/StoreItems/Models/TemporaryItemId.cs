using System;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models
{
    public readonly record struct TemporaryItemId
    {
        public TemporaryItemId() : this(Guid.Empty)
        {
        }

        public TemporaryItemId(Guid value)
        {
            if (value == Guid.Empty)
                throw new ArgumentException($"{nameof(value)} mustn't be default ({value})");

            Value = value;
        }

        public Guid Value { get; }
    }
}