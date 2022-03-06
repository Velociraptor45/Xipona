﻿using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared
{
    public class ItemAvailabilityContract
    {
        public Guid StoreId { get; set; }
        public float Price { get; set; }
        public Guid DefaultSectionId { get; set; }
    }
}