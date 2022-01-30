using System;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models
{
    public readonly record struct StoreId
    {
        public StoreId()
        {
            throw new NotSupportedException("Use 'New' property to create initial value.");
        }

        public StoreId(int value)
        {
            Value = value;
        }

        public static StoreId New => new(0);

        public int Value { get; }
    }
}