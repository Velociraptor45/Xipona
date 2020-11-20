﻿namespace ShoppingList.Api.Domain.Models
{
    public class StoreItemAvailability
    {
        public StoreItemAvailability(StoreId StoreId, float price)
        {
            this.StoreId = StoreId ?? throw new System.ArgumentNullException(nameof(StoreId));
            Price = price;
        }

        public StoreId StoreId { get; }
        public float Price { get; }
    }
}